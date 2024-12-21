public interface IUserTenantRepository
{
    Task<UserTenant> AddAsync(UserTenant userTenant);
    Task<bool> ExistsAsync(Guid userId, Guid tenantId);
    Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId);
    Task SaveChangesAsync();
    Task<UserTenant> GetByMobileNumberAsync(string mobileNumber);
  
}