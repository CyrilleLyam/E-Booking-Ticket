using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using server.src.Data;

namespace server.src.Workers;

public class ValveWorker : BackgroundService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IServiceScopeFactory _scopeFactory;
    private const int BatchSize = 100;

    public ValveWorker(IConnectionMultiplexer redis, IServiceScopeFactory scopeFactory)
    {
        _redis = redis;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var db = _redis.GetDatabase();
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var activeEventIds = await dbContext.Events
                    .Where(e => e.StartTime <= DateTime.UtcNow && e.StartTime > DateTime.UtcNow.AddHours(-2))
                    .Select(e => e.Id.ToString())
                    .ToListAsync(stoppingToken);

                foreach (var eventId in activeEventIds)
                {
                    await ProcessQueueForEvent(db, eventId);
                }
            }

            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task ProcessQueueForEvent(IDatabase db, string eventId)
    {
        var waitingKey = $"queue:{eventId}:waiting";
        var admittedKey = $"queue:{eventId}:admitted";

        var usersToAdmit = await db.SortedSetPopAsync(waitingKey, BatchSize);
        
        if (usersToAdmit != null && usersToAdmit.Length > 0)
        {
            var redisValues = usersToAdmit.Select(u => (RedisValue)u.Element).ToArray();
            
            await db.SetAddAsync(admittedKey, redisValues);
            
            await db.KeyExpireAsync(admittedKey, TimeSpan.FromMinutes(15));
        }
    }
}
