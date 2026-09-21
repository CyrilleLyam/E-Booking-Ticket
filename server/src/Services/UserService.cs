using MapsterMapper;
using server.src.Dtos;
using server.src.Models;
using server.src.Repositories;

namespace server.src.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        var users = await _userRepository.GetAll();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> Create(User user)
    {
        var created = await _userRepository.Create(user);
        return _mapper.Map<UserDto>(created);
    }
}
