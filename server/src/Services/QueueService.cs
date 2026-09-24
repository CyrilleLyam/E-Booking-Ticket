using StackExchange.Redis;
using server.src.Dtos;
using server.src.Repositories;

namespace server.src.Services;

public class QueueService : IQueueService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IEventRepository _eventRepository;

    public QueueService(IConnectionMultiplexer redis, IEventRepository eventRepository)
    {
        _redis = redis;
        _eventRepository = eventRepository;
    }

    public async Task<QueueStatusDto> JoinQueueAsync(int eventId, int userId)
    {
        var eventItem = await _eventRepository.GetById(eventId);
        if (eventItem == null)
        {
            throw new KeyNotFoundException($"Event with ID {eventId} not found.");
        }

        var db = _redis.GetDatabase();
        var waitingKey = $"queue:{eventId}:waiting";
        var admittedKey = $"queue:{eventId}:admitted";
        var userValue = userId.ToString();

        if (await db.SetContainsAsync(admittedKey, userValue))
        {
            return new QueueStatusDto
            {
                EventId = eventId,
                Status = "admitted",
                Position = null,
                Message = "You have already been admitted! You can proceed to select and book tickets."
            };
        }

        var existingRank = await db.SortedSetRankAsync(waitingKey, userValue);
        if (existingRank.HasValue)
        {
            var existingPosition = existingRank.Value + 1;
            return new QueueStatusDto
            {
                EventId = eventId,
                Status = "waiting",
                Position = existingPosition,
                Message = $"You are already in line. Your position is #{existingPosition}."
            };
        }

        var score = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await db.SortedSetAddAsync(waitingKey, userValue, score);

        var rank = await db.SortedSetRankAsync(waitingKey, userValue);
        var position = (rank ?? 0) + 1;

        return new QueueStatusDto
        {
            EventId = eventId,
            Status = "waiting",
            Position = position,
            Message = $"You have joined the queue! Your position is #{position}."
        };
    }

    public async Task<QueueStatusDto> GetQueueStatusAsync(int eventId, int userId)
    {
        var db = _redis.GetDatabase();
        var waitingKey = $"queue:{eventId}:waiting";
        var admittedKey = $"queue:{eventId}:admitted";
        var userValue = userId.ToString();

        if (await db.SetContainsAsync(admittedKey, userValue))
        {
            return new QueueStatusDto
            {
                EventId = eventId,
                Status = "admitted",
                Position = null,
                Message = "You are admitted! You can proceed to select and book tickets."
            };
        }

        var rank = await db.SortedSetRankAsync(waitingKey, userValue);
        if (rank.HasValue)
        {
            var position = rank.Value + 1;
            return new QueueStatusDto
            {
                EventId = eventId,
                Status = "waiting",
                Position = position,
                Message = $"You are #{position} in line. Please wait while we admit you."
            };
        }

        return new QueueStatusDto
        {
            EventId = eventId,
            Status = "not_in_queue",
            Position = null,
            Message = "You are not currently in the queue for this event."
        };
    }

    public async Task<bool> LeaveQueueAsync(int eventId, int userId)
    {
        var db = _redis.GetDatabase();
        var waitingKey = $"queue:{eventId}:waiting";
        var admittedKey = $"queue:{eventId}:admitted";
        var userValue = userId.ToString();

        var removedWaiting = await db.SortedSetRemoveAsync(waitingKey, userValue);
        var removedAdmitted = await db.SetRemoveAsync(admittedKey, userValue);

        return removedWaiting || removedAdmitted;
    }
}
