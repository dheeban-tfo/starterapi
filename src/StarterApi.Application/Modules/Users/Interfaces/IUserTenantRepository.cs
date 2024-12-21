using StarterApi.Domain.Entities;

public interface IUserTenantRepository
{
    Task<UserTenant> AddAsync(UserTenant userTenant);
    Task<bool> ExistsAsync(Guid userId, Guid tenantId);
    Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId);
    Task SaveChangesAsync();
    Task<UserTenant> GetByMobileNumberAsync(string mobileNumber);
    Task<UserTenant> GetByUserAndTenantIdAsync(Guid userId, Guid tenantId);
     Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, string permission);
    Task<List<Role>> GetTenantRolesAsync(Guid tenantId);
    Task<List<Permission>> GetTenantPermissionsAsync(Guid tenantId);
    Task<List<string>> GetUserPermissionsAsync(Guid userId, Guid tenantId);
}