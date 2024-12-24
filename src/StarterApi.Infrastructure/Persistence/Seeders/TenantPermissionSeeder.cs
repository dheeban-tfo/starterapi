using StarterApi.Domain.Constants;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace StarterApi.Infrastructure.Persistence.Seeders
{
    public class TenantPermissionSeeder
    {
        private readonly TenantDbContext _context;
        private readonly ILogger<TenantPermissionSeeder> _logger;

        public TenantPermissionSeeder(
            TenantDbContext context,
            ILogger<TenantPermissionSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (!_context.Permissions.Any())
            {
                _logger.LogInformation("Starting permission seeding");
                var permissions = new List<TenantPermission>();
                
                // Users permissions
                AddPermissionGroup(permissions, "Users", new[]
                {
                    (Permissions.Users.View, "View Users", "Can view users in the system"),
                    (Permissions.Users.Create, "Create Users", "Can create new users"),
                    (Permissions.Users.Edit, "Edit Users", "Can edit existing users"),
                    (Permissions.Users.Delete, "Delete Users", "Can delete users"),
                    (Permissions.Users.ManageRoles, "Manage User Roles", "Can manage user roles")
                });

                // Roles permissions
                AddPermissionGroup(permissions, "Roles", new[]
                {
                    (Permissions.Roles.View, "View Roles", "Can view roles in the system"),
                    (Permissions.Roles.Create, "Create Roles", "Can create new roles"),
                    (Permissions.Roles.Edit, "Edit Roles", "Can edit existing roles"),
                    (Permissions.Roles.Delete, "Delete Roles", "Can delete roles"),
                    (Permissions.Roles.ManagePermissions, "Manage Role Permissions", "Can manage role permissions")
                });

                await _context.Permissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Permission seeding completed. Added {Count} permissions", 
                    permissions.Count);
            }
            else
            {
                _logger.LogInformation("Permissions already exist, skipping seeding");
            }
        }

        private void AddPermissionGroup(List<TenantPermission> permissions, string group, 
            IEnumerable<(string SystemName, string Name, string Description)> groupPermissions)
        {
            foreach (var (systemName, name, description) in groupPermissions)
            {
                permissions.Add(new TenantPermission
                {
                    Id = Guid.NewGuid(),
                    SystemName = systemName,
                    Name = name,
                    Description = description,
                    Group = group,
                    IsEnabled = true,
                    IsSystem = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }
} 