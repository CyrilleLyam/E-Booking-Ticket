using server.src.Dtos;

namespace server.src.Services;

public interface IQueueService
{
    Task<QueueStatusDto> JoinQueueAsync(int eventId, int userId);
    Task<QueueStatusDto> GetQueueStatusAsync(int eventId, int userId);
    Task<bool> LeaveQueueAsync(int eventId, int userId);
}
