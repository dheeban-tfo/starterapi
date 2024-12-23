using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Constants;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Seeders
{
    public class RoleSeeder
    {
        private readonly RootDbContext _context;

        public RoleSeeder(RootDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(Guid tenantId)
        {
            if (!await _context.Roles.AnyAsync(r => r.TenantId == tenantId))
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Root Admin",
                        Description = "Super administrator with full system access",
                        
                        TenantId = tenantId,
                        CreatedBy = tenantId,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                // Add roles
                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();

                // Assign all permissions to RootAdmin
                var permissions = await _context.Permissions.ToListAsync();
                var rolePermissions = permissions.Select(p => new RolePermission
                {
                    RoleId = roles[0].Id,
                    PermissionId = p.Id,
                    CreatedBy = tenantId,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.RolePermissions.AddRangeAsync(rolePermissions);
                await _context.SaveChangesAsync();
            }
        }
    }
}