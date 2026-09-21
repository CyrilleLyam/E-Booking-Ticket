using server.src.Dtos;
using server.src.Models;

namespace server.src.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto?> GetById(int id);
    Task<UserDto> Create(User user);
}
