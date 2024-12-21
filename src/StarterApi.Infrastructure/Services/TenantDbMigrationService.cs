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
            _logger.LogInformation("Seeding roles data");
            var roles = new[]
            {
                new TenantRole { Name = "Admin" },
                new TenantRole { Name = "User" }
            };
            
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
            _logger.LogInformation("Roles seeded successfully");
        }
    }
} 