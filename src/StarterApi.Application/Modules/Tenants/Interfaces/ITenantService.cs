public interface ITenantService
{
    Task<TenantDto> CreateTenantAsync(CreateTenantDto dto);
    Task<TenantDto> GetTenantByIdAsync(Guid id);
    Task<IEnumerable<TenantDto>> GetAllTenantsAsync();
    Task DeactivateTenantAsync(Guid id);
} 