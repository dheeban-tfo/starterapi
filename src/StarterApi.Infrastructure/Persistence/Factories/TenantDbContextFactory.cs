using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Factories
{
    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            return new TenantDbContext("Server=localhost;Database=Tenant_Template;User Id=sa;Password=MyPass@word;TrustServerCertificate=True;MultipleActiveResultSets=true;");
        }
    }
} 