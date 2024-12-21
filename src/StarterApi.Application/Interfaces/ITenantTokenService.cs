public interface ITenantTokenService
{
    Task<string> GenerateTenantTokenAsync(User user, Guid tenantId);
} 