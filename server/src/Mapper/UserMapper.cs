using Mapster;
using server.src.Dtos;
using server.src.Models;

namespace server.src.Mapper;

public class UserMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterDto, User>()
            .Ignore(dest => dest.Password)
            .Map(dest => dest.Role, src => src.Role ?? UserRole.User);

        config.NewConfig<User, UserDto>();
    }
}
