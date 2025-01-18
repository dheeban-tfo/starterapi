using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StarterApi.Infrastructure.Persistence.Seeders;

public class RootDataSeeder
{
    private readonly RootDbContext _context;
    private readonly IUserTenantRepository _userTenantRepository;
    private readonly ITenantDbMigrationService _tenantDbMigrationService;
    private readonly ILogger<RootDataSeeder> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public RootDataSeeder(
        RootDbContext context,
        IUserTenantRepository userTenantRepository,
        ITenantDbMigrationService tenantDbMigrationService,
        ILogger<RootDataSeeder> logger,
        ILoggerFactory loggerFactory)
    {
        _context = context;
        _userTenantRepository = userTenantRepository;
        _tenantDbMigrationService = tenantDbMigrationService;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task SeedAsync()
    {
        try
        {
            // 1. Seed permissions first
            var permissionSeeder = new PermissionSeeder(_context, _loggerFactory.CreateLogger<PermissionSeeder>());
            await permissionSeeder.SeedAsync();
            _logger.LogInformation("Permission seeding completed");

            // 2. Create RootAdmin user first
            var rootAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Email == "rootadmin@example.com");
            if (rootAdmin == null)
            {
                Guid rootUserId = Guid.NewGuid();
                rootAdmin = new User
                {
                    Id = rootUserId,
                    Email = "rootadmin@example.com",
                    FirstName = "Root",
                    LastName = "Admin",
                    MobileNumber = "1234567890",
                    UserType = UserType.RootAdmin,
                    IsActive = true,
                    CreatedBy= rootUserId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(rootAdmin);
                await _context.SaveChangesAsync();
            }

            // 3. Create default tenants first
            var tenants = new[]
            {
                new { Name = "Alpha", DbName = "Alpha_Tenant", },
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
                        ConnectionString = $"Server=localhost;Database={t.DbName};User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true;",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = rootAdmin.Id
                    };
                    _context.Tenants.Add(tenant);
                    await _context.SaveChangesAsync();

                    // 4. Create tenant database
                    await _tenantDbMigrationService.CreateTenantDatabaseAsync(tenant);
                    _logger.LogInformation("Created database for tenant: {TenantName}", t.Name);

                    // 5. Seed roles for the tenant after database is created
                    var roleSeeder = new RoleSeeder(_context);
                    await roleSeeder.SeedAsync(tenant.Id);
                    _logger.LogInformation("Role seeding completed for tenant: {TenantName}", t.Name);

                    // 6. Map RootAdmin to tenant with Root Admin role
                    if (!await _userTenantRepository.ExistsAsync(rootAdmin.Id, tenant.Id))
                    {
                        var userTenant = new UserTenant
                        {
                            UserId = rootAdmin.Id,
                            TenantId = tenant.Id,
                            RoleId = await GetRootAdminRoleId(tenant.Id),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = rootAdmin.Id
                            
                        };
                        await _userTenantRepository.AddAsync(userTenant);
                        await _userTenantRepository.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }

    private async Task<Guid> GetRootAdminRoleId(Guid tenantId)
    {
        var rootAdminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.Name == "Root Admin");
        return rootAdminRole?.Id ?? throw new InvalidOperationException("Root Admin role not found");
    }
}