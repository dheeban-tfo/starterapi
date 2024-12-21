using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TenantDbMigrationService : ITenantDbMigrationService
{
    private readonly ILogger<TenantDbMigrationService> _logger;

    public TenantDbMigrationService(ILogger<TenantDbMigrationService> logger)
    {
        _logger = logger;
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

        // Create default TenantAdmin user
        var tenantAdmin = new TenantUser
        {
            Email = "tenantadmin@example.com",
            FirstName = "Tenant",
            LastName = "Admin",
            MobileNumber = "1234567890",
            RoleId = tenantAdminRole.Id,
            CreatedAt = DateTime.UtcNow
        };

        // Create default User
        var defaultUser = new TenantUser
        {
            Email = "user@example.com",
            FirstName = "Default",
            LastName = "User",
            MobileNumber = "0987654321", 
            RoleId = userRole.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.AddRange(new[] { tenantAdmin, defaultUser });
        await context.SaveChangesAsync();

        _logger.LogInformation("Tenant roles and users seeded successfully");
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
            TenantRoleId = roleId,
            CreatedAt = DateTime.UtcNow
        };
    }
} 