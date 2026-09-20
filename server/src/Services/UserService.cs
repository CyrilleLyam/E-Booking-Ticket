using server.src.Models;
using server.src.Repositories;

namespace server.src.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> GetById(int id)
    {
        return await _userRepository.GetById(id);
    }

    public async Task<User> Create(User user)
    {
        return await _userRepository.Create(user);
    }
}
