using Mapster;
using server.src.Dtos;
using server.src.Models;

namespace server.src.Mapper;

public class EventMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateEventDto, Event>()
            .Map(dest => dest.AvailableSeats, src => src.TotalSeats);
        config.NewConfig<UpdateEventDto, Event>()
            .IgnoreNullValues(true);
        config.NewConfig<Event, EventResponseDto>();
    }
}
