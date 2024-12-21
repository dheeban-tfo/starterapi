using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces
{
    public interface ITenantDbContext
    {
        DbSet<TenantUser> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 