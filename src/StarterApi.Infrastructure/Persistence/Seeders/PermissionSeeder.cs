using StarterApi.Domain.Constants;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Seeders
{
    public class PermissionSeeder
    {
        private readonly RootDbContext _context;

        public PermissionSeeder(RootDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Permissions.Any())
            {
                var permissions = new List<Permission>();
                
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

                // Tenants permissions
                AddPermissionGroup(permissions, "Tenants", new[]
                {
                    (Permissions.Tenants.View, "View Tenants", "Can view tenants in the system"),
                    (Permissions.Tenants.Create, "Create Tenants", "Can create new tenants"),
                    (Permissions.Tenants.Edit, "Edit Tenants", "Can edit existing tenants"),
                    (Permissions.Tenants.Delete, "Delete Tenants", "Can delete tenants"),
                    (Permissions.Tenants.ManageUsers, "Manage Tenant Users", "Can manage tenant users")
                });

                await _context.Permissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
            }
        }

        private void AddPermissionGroup(List<Permission> permissions, string group, 
            IEnumerable<(string SystemName, string Name, string Description)> groupPermissions)
        {
            foreach (var (systemName, name, description) in groupPermissions)
            {
                permissions.Add(new Permission
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