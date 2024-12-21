using System.Security.Claims;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateBaseToken(User user);
        Task<string> GenerateTenantTokenAsync(User user, Guid tenantId);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateToken(string token);
    }
} 