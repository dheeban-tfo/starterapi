using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces
{
    public interface ITenantDbContext
    {
        DbSet<TenantUser> Users { get; set; }
        DbSet<TenantRole> Roles { get; set; }
        DbSet<TenantPermission> Permissions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 