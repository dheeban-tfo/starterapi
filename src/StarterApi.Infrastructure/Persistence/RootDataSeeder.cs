using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;


public class RootDataSeeder
{
    private readonly RootDbContext _context;
    private readonly IUserTenantRepository _userTenantRepository;
    private readonly ITenantDbMigrationService _tenantDbMigrationService;
    private readonly ILogger<RootDataSeeder> _logger;

    public RootDataSeeder(
        RootDbContext context,
        IUserTenantRepository userTenantRepository,
        ITenantDbMigrationService tenantDbMigrationService,
        ILogger<RootDataSeeder> logger)
    {
        _context = context;
        _userTenantRepository = userTenantRepository;
        _tenantDbMigrationService = tenantDbMigrationService;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        // Create admin user if not exists
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@example.com");
        if (adminUser == null)
        {
            adminUser = new User
            {
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = "hashed_password",
                UserType = UserType.TenantAdmin,
                IsActive = true
            };
            _context.Users.Add(adminUser);
        }

        // Create default tenants if they don't exist
        var tenants = new[]
        {
           
            new { Name = "Alpha", DbName = "Alpha_Tenant" },
            new { Name = "Beta", DbName = "Beta_Tenant" }
        };

        foreach (var t in tenants)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Name == t.Name);
            if (tenant == null)
            {
                tenant = new Tenant
                {
                    Name = t.Name,
                    DatabaseName = t.DbName,
                    Status = TenantStatus.Active,
                    ConnectionString = $"Server=localhost;Database={t.DbName};User Id=sa;Password=MyPass@word;TrustServerCertificate=True;MultipleActiveResultSets=true;"
                };
                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                // Create the tenant database
                try
                {
                    await _tenantDbMigrationService.CreateTenantDatabaseAsync(tenant);
                    _logger.LogInformation("Created database for tenant: {TenantName}", t.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create database for tenant: {TenantName}", t.Name);
                    throw;
                }
            }

            // Add admin user to each tenant
            if (!await _userTenantRepository.ExistsAsync(adminUser.Id, tenant.Id))
            {
                var userTenant = new UserTenant(adminUser, tenant, Guid.Parse("d42868ae-b072-486d-9077-d0d527f3ba39"));
                await _userTenantRepository.AddAsync(userTenant);
                await _userTenantRepository.SaveChangesAsync();
            }
        }
    }
} 