public interface ITenantService
{
    Task<TenantDto> CreateTenantAsync(CreateTenantDto dto);
    Task<TenantInternalDto> GetTenantByIdAsync(Guid id);
    Task<IEnumerable<TenantDto>> GetAllTenantsAsync();
    Task DeactivateTenantAsync(Guid id);
    Task<bool> DeleteTenantAsync(Guid id);
} 