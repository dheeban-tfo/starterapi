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
        try
        {
            _logger.LogInformation("Creating database for tenant {TenantName}", tenant.Name);

            using var context = new TenantDbContext(tenant.ConnectionString);

            // Check if database exists
            bool dbExists = await context.Database.CanConnectAsync();
            
            if (!dbExists)
            {
                // Apply migrations to create the database with schema
                await context.Database.MigrateAsync();
                
                // Seed initial data
                await SeedTenantDataAsync(context);
                
                _logger.LogInformation("Successfully created database for tenant {TenantName}", tenant.Name);
            }
            else
            {
                _logger.LogInformation("Database already exists for tenant {TenantName}", tenant.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating database for tenant {TenantName}", tenant.Name);
            throw;
        }
    }

    private async Task SeedTenantDataAsync(TenantDbContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = new[]
            {
                new TenantRole { Name = "Admin" },
                new TenantRole { Name = "User" }
            };

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
            
            _logger.LogInformation("Seeded default roles");
        }
    }
} 