using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.src.Dtos;
using server.src.Services;

namespace server.src.Controllers;

[Authorize]
[ApiController]
[Route("api/queue")]
public class QueueController : ControllerBase
{
    private readonly IQueueService _queueService;

    public QueueController(IQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpPost("{eventId:int}/join")]
    public async Task<ActionResult<BaseResponse<QueueStatusDto>>> JoinQueue(int eventId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = true, status = StatusCodes.Status401Unauthorized, message = "User not identified." });
        }

        var result = await _queueService.JoinQueueAsync(eventId, userId.Value);
        return Ok(new BaseResponse<QueueStatusDto>(result));
    }

    [HttpGet("{eventId:int}/status")]
    public async Task<ActionResult<BaseResponse<QueueStatusDto>>> GetQueueStatus(int eventId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = true, status = StatusCodes.Status401Unauthorized, message = "User not identified." });
        }

        var result = await _queueService.GetQueueStatusAsync(eventId, userId.Value);
        return Ok(new BaseResponse<QueueStatusDto>(result));
    }

    [HttpDelete("{eventId:int}/leave")]
    public async Task<ActionResult<BaseResponse<object>>> LeaveQueue(int eventId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized(new { error = true, status = StatusCodes.Status401Unauthorized, message = "User not identified." });
        }

        var removed = await _queueService.LeaveQueueAsync(eventId, userId.Value);
        return Ok(new BaseResponse<object>(
            data: new { left = removed },
            message: removed ? "Successfully left the queue." : "You were not in the queue."
        ));
    }

    private int? GetCurrentUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
