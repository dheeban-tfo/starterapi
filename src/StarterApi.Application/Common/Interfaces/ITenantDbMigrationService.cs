using StarterApi.Domain.Entities;

public interface ITenantDbMigrationService
{
    Task CreateTenantDatabaseAsync(Tenant tenant);
    Task UpdateAllTenantDatabasesAsync(IEnumerable<Tenant> tenants);
} 