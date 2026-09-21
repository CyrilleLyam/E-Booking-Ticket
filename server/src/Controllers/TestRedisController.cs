using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

[ApiController]
[Route("api/[controller]")]
public class TestRedisController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;

    public TestRedisController(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    [HttpGet]
    public async Task<IActionResult> TestRedis()
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync("test", "Hello, Redis!");
        var value = await db.StringGetAsync("test");
        return Ok(new { message = value.ToString() });
    }
}
