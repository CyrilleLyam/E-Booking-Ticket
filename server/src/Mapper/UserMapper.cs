using Mapster;
using server.src.Dtos;
using server.src.Models;

namespace server.src.Mapper;

public class UserMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterDto, User>();
        config.NewConfig<User, UserDto>();
    }
}
