using server.src.Models;

namespace server.src.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByToken(string token);
    Task<RefreshToken> Create(RefreshToken refreshToken);
    Task Update(RefreshToken refreshToken);
}
