public interface IRefreshTokenRepository
{
    Task<RefreshToken> AddAsync(RefreshToken refreshToken);
    Task<RefreshToken> GetByTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
    Task RevokeAsync(string token);
    Task SaveChangesAsync();
} 