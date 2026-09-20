using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.src.Dtos;
using server.src.Services;

namespace server.src.Controllers;

[Authorize]
[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<EventResponseDto>>>> GetAll([FromQuery] EventQueryDto query)
    {
        var events = await _eventService.GetAll(query);
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseResponse<EventResponseDto>>> GetById(int id)
    {
        var eventItem = await _eventService.GetById(id);
        if (eventItem == null)
        {
            return NotFound(new { error = true, status = StatusCodes.Status404NotFound, message = "Event not found." });
        }
        return Ok(new BaseResponse<EventResponseDto>(eventItem));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<ActionResult<BaseResponse<EventResponseDto>>> Create([FromBody] CreateEventDto createEventDto)
    {
        var created = await _eventService.Create(createEventDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new BaseResponse<EventResponseDto>(created));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Organizer")]
    public async Task<ActionResult<BaseResponse<EventResponseDto>>> Update(int id, [FromBody] UpdateEventDto updateEventDto)
    {
        var updated = await _eventService.Update(id, updateEventDto);
        if (updated == null)
        {
            return NotFound(new { error = true, status = StatusCodes.Status404NotFound, message = "Event not found." });
        }
        return Ok(new BaseResponse<EventResponseDto>(updated));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _eventService.Delete(id);
        if (!deleted)
        {
            return NotFound(new { error = true, status = StatusCodes.Status404NotFound, message = "Event not found." });
        }
        return NoContent();
    }
}