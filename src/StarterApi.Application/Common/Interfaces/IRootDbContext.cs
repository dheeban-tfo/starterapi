using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces
{
    public interface IRootDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<Permission> Permissions { get; }
        DbSet<RolePermission> RolePermissions { get; }
        DbSet<Tenant> Tenants { get; }
        DbSet<UserTenant> UserTenants { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 