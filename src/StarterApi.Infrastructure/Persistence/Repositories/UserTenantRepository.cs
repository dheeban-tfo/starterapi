using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class UserTenantRepository : IUserTenantRepository
    {
        private readonly RootDbContext _context;

        public UserTenantRepository(RootDbContext context)
        {
            _context = context;
        }

        public async Task<UserTenant> AddAsync(UserTenant userTenant)
        {
            await _context.UserTenants.AddAsync(userTenant);
            return userTenant;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid tenantId)
        {
            return await _context.UserTenants
                .AnyAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
        }

        public async Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserTenants
                .Include(ut => ut.Tenant)
                .Where(ut => ut.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _context.UserTenants
                .Include(ut => ut.User)
                .Where(ut => ut.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
