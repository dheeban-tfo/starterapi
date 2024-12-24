using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Domain.Constants;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Seeders
{
    public class PermissionSeeder
    {
        private readonly RootDbContext _context;
        private readonly ILogger<PermissionSeeder> _logger;

        public PermissionSeeder(
            RootDbContext context,
            ILogger<PermissionSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting permission seeding/update");
                var permissions = new List<Permission>();
                var existingPermissions = await _context.Permissions.ToListAsync();
                
                // Users permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Users", new[]
                {
                    (Permissions.Users.View, "View Users", "Can view users in the system"),
                    (Permissions.Users.Create, "Create Users", "Can create new users"),
                    (Permissions.Users.Edit, "Edit Users", "Can edit existing users"),
                    (Permissions.Users.Delete, "Delete Users", "Can delete users"),
                    (Permissions.Users.ManageRoles, "Manage User Roles", "Can manage user roles"),
                    (Permissions.Users.InviteUser, "Invite Users", "Can invite new users to the system")
                });

                // Roles permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Roles", new[]
                {
                    (Permissions.Roles.View, "View Roles", "Can view roles in the system"),
                    (Permissions.Roles.Create, "Create Roles", "Can create new roles"),
                    (Permissions.Roles.Edit, "Edit Roles", "Can edit existing roles"),
                    (Permissions.Roles.Delete, "Delete Roles", "Can delete roles"),
                    (Permissions.Roles.ManagePermissions, "Manage Role Permissions", "Can manage role permissions")
                });

                // Tenants permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Tenants", new[]
                {
                    (Permissions.Tenants.View, "View Tenants", "Can view tenants in the system"),
                    (Permissions.Tenants.Create, "Create Tenants", "Can create new tenants"),
                    (Permissions.Tenants.Edit, "Edit Tenants", "Can edit existing tenants"),
                    (Permissions.Tenants.Delete, "Delete Tenants", "Can delete tenants"),
                    (Permissions.Tenants.ManageUsers, "Manage Tenant Users", "Can manage tenant users")
                });

                // Societies permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Societies", new[]
                {
                    (Permissions.Societies.View, "View Societies", "Can view societies in the system"),
                    (Permissions.Societies.Create, "Create Societies", "Can create new societies"),
                    (Permissions.Societies.Edit, "Edit Societies", "Can edit existing societies"),
                    (Permissions.Societies.Delete, "Delete Societies", "Can delete societies"),
                    (Permissions.Societies.ManageBlocks, "Manage Society Blocks", "Can manage society blocks")
                });

                // Blocks permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Blocks", new[]
                {
                    (Permissions.Blocks.View, "View Blocks", "Can view blocks in the system"),
                    (Permissions.Blocks.Create, "Create Blocks", "Can create new blocks"),
                    (Permissions.Blocks.Edit, "Edit Blocks", "Can edit existing blocks"),
                    (Permissions.Blocks.Delete, "Delete Blocks", "Can delete blocks"),
                    (Permissions.Blocks.ManageFloors, "Manage Block Floors", "Can manage block floors")
                });

                // Floors permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Floors", new[]
                {
                    (Permissions.Floors.View, "View Floors", "Can view floors in the system"),
                    (Permissions.Floors.Create, "Create Floors", "Can create new floors"),
                    (Permissions.Floors.Edit, "Edit Floors", "Can edit existing floors"),
                    (Permissions.Floors.Delete, "Delete Floors", "Can delete floors"),
                    (Permissions.Floors.ManageUnits, "Manage Floor Units", "Can manage floor units")
                });

                // Units permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Units", new[]
                {
                    (Permissions.Units.View, "View Units", "Can view units in the system"),
                    (Permissions.Units.Create, "Create Units", "Can create new units"),
                    (Permissions.Units.Edit, "Edit Units", "Can edit existing units"),
                    (Permissions.Units.Delete, "Delete Units", "Can delete units")
                });

                if (permissions.Any())
                {
                    await _context.Permissions.AddRangeAsync(permissions);
                    await _context.SaveChangesAsync();
                    
                    // Assign new permissions to Root Admin role
                    var rootAdminRole = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Name == "Root Admin");
                    
                    if (rootAdminRole != null)
                    {
                        var existingRolePermissions = await _context.RolePermissions
                            .Where(rp => rp.RoleId == rootAdminRole.Id)
                            .Select(rp => rp.PermissionId)
                            .ToListAsync();

                        var newRolePermissions = permissions
                            .Where(p => !existingRolePermissions.Contains(p.Id))
                            .Select(p => new RolePermission
                            {
                                RoleId = rootAdminRole.Id,
                                PermissionId = p.Id,
                                CreatedBy = rootAdminRole.CreatedBy,
                                CreatedAt = DateTime.UtcNow
                            });

                        await _context.RolePermissions.AddRangeAsync(newRolePermissions);
                        await _context.SaveChangesAsync();
                    }
                }
                
                _logger.LogInformation("Permission seeding/update completed. Added {Count} new permissions", 
                    permissions.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding permissions");
                throw;
            }
        }

        private async Task AddOrUpdatePermissionGroup(
            List<Permission> newPermissions,
            List<Permission> existingPermissions,
            string group,
            IEnumerable<(string SystemName, string Name, string Description)> groupPermissions)
        {
            foreach (var (systemName, name, description) in groupPermissions)
            {
                if (!existingPermissions.Any(p => p.SystemName == systemName))
                {
                    newPermissions.Add(new Permission
                    {
                        Id = Guid.NewGuid(),
                        SystemName = systemName,
                        Name = name,
                        Description = description,
                        Group = group,
                        IsSystem = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
} 