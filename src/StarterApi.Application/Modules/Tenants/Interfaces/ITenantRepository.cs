using StarterApi.Domain.Entities;

public interface ITenantRepository
{
    Task<Tenant> GetByIdAsync(Guid id);
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<Tenant> AddAsync(Tenant tenant);
    Task SaveChangesAsync();
} 