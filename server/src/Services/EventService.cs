using MapsterMapper;
using server.src.Dtos;
using server.src.Models;
using server.src.Repositories;

namespace server.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<IEnumerable<EventResponseDto>>> GetAll(EventQueryDto query)
    {
        var (items, totalCount) = await _eventRepository.GetAll(query);

        return new BaseResponse<IEnumerable<EventResponseDto>>(
            items,
            new PaginationMeta
            {
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            }
        );
    }

    public async Task<EventResponseDto?> GetById(int id)
    {
        var eventEntity = await _eventRepository.GetById(id);
        return eventEntity == null ? null : _mapper.Map<EventResponseDto>(eventEntity);
    }

    public async Task<EventResponseDto> Create(CreateEventDto createEventDto)
    {
        var eventEntity = _mapper.Map<Event>(createEventDto);
        var created = await _eventRepository.Create(eventEntity);
        return _mapper.Map<EventResponseDto>(created);
    }

    public async Task<EventResponseDto?> Update(int id, UpdateEventDto updateEventDto)
    {
        var existing = await _eventRepository.GetById(id);
        if (existing == null)
        {
            return null;
        }

        _mapper.Map(updateEventDto, existing);
        await _eventRepository.Update(existing);
        return _mapper.Map<EventResponseDto>(existing);
    }

    public async Task<bool> Delete(int id)
    {
        var existing = await _eventRepository.GetById(id);
        if (existing == null)
        {
            return false;
        }

        await _eventRepository.Delete(id);
        return true;
    }
}
