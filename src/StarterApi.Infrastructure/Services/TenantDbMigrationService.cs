using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Infrastructure.Persistence.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TenantDbMigrationService : ITenantDbMigrationService
{
    private readonly ILogger<TenantDbMigrationService> _logger;
    private readonly ILogger<TenantPermissionSeeder> _permissionSeederLogger;

    public TenantDbMigrationService(
        ILogger<TenantDbMigrationService> logger,
        ILogger<TenantPermissionSeeder> permissionSeederLogger)
    {
        _logger = logger;
        _permissionSeederLogger = permissionSeederLogger;
    }

    public async Task CreateTenantDatabaseAsync(Tenant tenant)
    {
        _logger.LogInformation("Starting database setup for tenant: {TenantName}", tenant.Name);
        
        try
        {
            using var context = new TenantDbContext(tenant.ConnectionString);
            
            // Check if the database exists
            bool dbExists = await context.Database.CanConnectAsync();
            bool hasMigrationsHistory = await CheckMigrationsTableExistsAsync(context);

            if (!dbExists)
            {
                _logger.LogInformation("Creating new database for tenant: {TenantName}", tenant.Name);
                await context.Database.MigrateAsync();
                await EnsureRolesSeededAsync(context);
            }
            else if (!hasMigrationsHistory)
            {
                _logger.LogInformation("Database exists but needs migration setup for tenant: {TenantName}", tenant.Name);
                // Drop and recreate the database to handle existing tables
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
                await EnsureRolesSeededAsync(context);
            }
            else
            {
                _logger.LogInformation("Checking pending migrations for tenant: {TenantName}", tenant.Name);
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }
                await EnsureRolesSeededAsync(context);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up database for tenant: {TenantName}", tenant.Name);
            throw;
        }
    }

    private async Task<bool> CheckMigrationsTableExistsAsync(TenantDbContext context)
    {
        try
        {
            var conn = context.Database.GetDbConnection();
            await using var command = conn.CreateCommand();
            command.CommandText = @"
                SELECT CASE 
                    WHEN EXISTS (
                        SELECT 1 
                        FROM sys.tables 
                        WHERE name = '__EFMigrationsHistory'
                    ) THEN 1
                    ELSE 0
                END";

            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return Convert.ToBoolean(result);
        }
        catch
        {
            return false;
        }
    }

    private async Task EnsureRolesSeededAsync(TenantDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            _logger.LogInformation("Seeding tenant roles and users");

            // First seed permissions
            var permissionSeeder = new TenantPermissionSeeder(context, _permissionSeederLogger);
            await permissionSeeder.SeedAsync();
            _logger.LogInformation("Tenant permissions seeded successfully");

            // Create TenantAdmin role
            var tenantAdminRole = new TenantRole
            {
                Name = "Tenant Admin",
                Description = "Tenant administrator with full tenant access",
                CreatedAt = DateTime.UtcNow
            };

            // Create User role
            var userRole = new TenantRole
            {
                Name = "User",
                Description = "Regular tenant user",
                CreatedAt = DateTime.UtcNow
            };

            context.Roles.AddRange(new[] { tenantAdminRole, userRole });
            await context.SaveChangesAsync();
            _logger.LogInformation("Tenant roles created successfully");

            // Assign permissions to roles through RolePermissions
            var permissions = await context.Permissions.ToListAsync();
            
            // Assign all permissions to Tenant Admin
            var adminRolePermissions = permissions.Select(p => new TenantRolePermission
            {
                RoleId = tenantAdminRole.Id,
                PermissionId = p.Id,
                CreatedAt = DateTime.UtcNow
            });
            
            // Assign basic permissions to User role
            var userPermissions = permissions
                .Where(p => p.SystemName == "Users.View" || p.SystemName == "Roles.View")
                .Select(p => new TenantRolePermission
                {
                    RoleId = userRole.Id,
                    PermissionId = p.Id,
                    CreatedAt = DateTime.UtcNow
                });

            await context.RolePermissions.AddRangeAsync(adminRolePermissions);
            await context.RolePermissions.AddRangeAsync(userPermissions);
            await context.SaveChangesAsync();
            _logger.LogInformation("Role permissions assigned successfully");
        }
    }

    private IEnumerable<TenantPermission> GetDefaultAdminPermissions(Guid roleId)
    {
        return new[]
        {
            CreatePermission("Users.View", "View Users", "Users", roleId),
            CreatePermission("Users.Create", "Create Users", "Users", roleId),
            CreatePermission("Users.Edit", "Edit Users", "Users", roleId),
            CreatePermission("Users.Delete", "Delete Users", "Users", roleId),
            CreatePermission("Roles.View", "View Roles", "Roles", roleId),
            CreatePermission("Roles.Manage", "Manage Roles", "Roles", roleId)
        };
    }

    private IEnumerable<TenantPermission> GetDefaultUserPermissions(Guid roleId)
    {
        return new[]
        {
            CreatePermission("Users.View", "View Users", "Users", roleId),
            CreatePermission("Roles.View", "View Roles", "Roles", roleId)
        };
    }

    private TenantPermission CreatePermission(string systemName, string name, string group, Guid roleId)
    {
        return new TenantPermission
        {
            SystemName = systemName,
            Name = name,
            Group = group,
            IsEnabled = true,
            IsSystem = true,
          
            CreatedAt = DateTime.UtcNow
        };
    }
} 