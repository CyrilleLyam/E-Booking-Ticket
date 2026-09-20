using server.src.Models;

namespace server.src.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(int id);
    Task<User> Create(User user);
}
