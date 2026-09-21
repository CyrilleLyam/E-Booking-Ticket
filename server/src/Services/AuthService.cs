using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MapsterMapper;
using Microsoft.IdentityModel.Tokens;
using server.src.Config;
using server.src.Dtos;
using server.src.Models;
using server.src.Repositories;

namespace server.src.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Register(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByEmail(registerDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = _mapper.Map<User>(registerDto);
        user.Password = _passwordHasher.Hash(registerDto.Password);

        var createdUser = await _userRepository.Create(user);
        return await GenerateAuthResponse(createdUser);
    }

    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmail(loginDto.Email);
        if (user == null || !_passwordHasher.Verify(user.Password, loginDto.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return await GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var storedToken = await _refreshTokenRepository.GetByToken(refreshTokenDto.RefreshToken);
        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            throw new SecurityTokenException("Invalid or expired refresh token.");
        }

        storedToken.IsRevoked = true;
        await _refreshTokenRepository.Update(storedToken);

        return await GenerateAuthResponse(storedToken.User);
    }

    public async Task RevokeToken(RefreshTokenDto refreshTokenDto)
    {
        var storedToken = await _refreshTokenRepository.GetByToken(refreshTokenDto.RefreshToken);
        if (storedToken == null || storedToken.IsRevoked)
        {
            return;
        }

        storedToken.IsRevoked = true;
        await _refreshTokenRepository.Update(storedToken);
    }

    private async Task<AuthResponseDto> GenerateAuthResponse(User user)
    {
        var minutes = EnvValidator.GetRequiredInt("ACCESS_TOKEN_EXPIRATION_MINUTES");
        var accessExpires = DateTime.UtcNow.AddMinutes(minutes);
        var accessToken = GenerateAccessToken(user, accessExpires);

        var days = EnvValidator.GetRequiredInt("REFRESH_TOKEN_EXPIRATION_DAYS");
        var refreshExpires = DateTime.UtcNow.AddDays(days);
        var refreshToken = new RefreshToken
        {
            Token = GenerateSecureRandomToken(),
            ExpiresAt = refreshExpires,
            IsRevoked = false,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.Create(refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiresAt = accessExpires,
            RefreshTokenExpiresAt = refreshExpires,
            User = _mapper.Map<UserDto>(user)
        };
    }

    private string GenerateAccessToken(User user, DateTime expires)
    {
        var secret = EnvValidator.GetRequired("JWT_SECRET");
        var issuer = EnvValidator.GetRequired("JWT_ISSUER");
        var audience = EnvValidator.GetRequired("JWT_AUDIENCE");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateSecureRandomToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
