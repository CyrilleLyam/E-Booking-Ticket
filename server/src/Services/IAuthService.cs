using server.src.Dtos;

namespace server.src.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterDto registerDto);
    Task<AuthResponseDto> Login(LoginDto loginDto);
    Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task RevokeToken(RefreshTokenDto refreshTokenDto);
}