using server.src.Dtos;

namespace server.src.Services;

public interface IEventService
{
    Task<BaseResponse<IEnumerable<EventResponseDto>>> GetAll(EventQueryDto query);
    Task<EventResponseDto?> GetById(int id);
    Task<EventResponseDto> Create(CreateEventDto createEventDto);
    Task<EventResponseDto?> Update(int id, UpdateEventDto updateEventDto);
    Task<bool> Delete(int id);
}
