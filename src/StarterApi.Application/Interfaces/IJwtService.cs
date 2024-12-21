using StarterApi.Domain.Entities;

namespace StarterApi.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(User user, Guid tenantId);
        string GenerateRefreshToken();
    }
} 