using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Domain.Entities;


public class RootDataSeeder
{
    private readonly RootDbContext _context;
    private readonly TenantDbMigrationService _tenantDbMigrationService;
    private readonly ILogger<RootDataSeeder> _logger;

    public RootDataSeeder(
        RootDbContext context,
        TenantDbMigrationService tenantDbMigrationService,
        ILogger<RootDataSeeder> logger)
    {
        _context = context;
        _tenantDbMigrationService = tenantDbMigrationService;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!_context.Tenants.Any())
            {
                _logger.LogInformation("Seeding initial tenants...");

                // Create default tenants
                var alphaTenant = new Tenant("Alpha", "Tenant_Alpha");
                var betaTenant = new Tenant("Beta", "Tenant_Beta");

                _context.Tenants.AddRange(alphaTenant, betaTenant);
                await _context.SaveChangesAsync();

                // Create tenant databases
                await _tenantDbMigrationService.CreateTenantDatabaseAsync(alphaTenant);
                await _tenantDbMigrationService.CreateTenantDatabaseAsync(betaTenant);

                // Create root admin user
                var rootAdmin = new User(
                    "root@admin.com",
                    "hashed_password",
                    "Root",
                    "Admin",
                    UserType.RootAdmin
                );

                _context.Users.Add(rootAdmin);

                // Give root admin access to both tenants
                rootAdmin.AddTenant(alphaTenant, Guid.NewGuid());
                rootAdmin.AddTenant(betaTenant, Guid.NewGuid());

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding root database");
            throw;
        }
    }
} 