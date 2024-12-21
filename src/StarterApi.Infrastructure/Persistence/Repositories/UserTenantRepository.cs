using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class UserTenantRepository : IUserTenantRepository
    {
        private readonly RootDbContext _context;
        private readonly ILogger<UserTenantRepository> _logger;

        public UserTenantRepository(
            RootDbContext context,
            ILogger<UserTenantRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserTenant> GetByMobileNumberAsync(string mobileNumber)
        {
            _logger.LogInformation("Looking up UserTenant by mobile number: {MobileNumber}", mobileNumber);

            var userTenant = await _context.UserTenants
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.User.MobileNumber == mobileNumber);

            _logger.LogInformation("UserTenant lookup result: {Found}", userTenant != null);
            return userTenant;
        }

        public async Task<UserTenant> AddAsync(UserTenant userTenant)
        {
            await _context.UserTenants.AddAsync(userTenant);
            return userTenant;
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

        public async Task<bool> ExistsAsync(Guid userId, Guid tenantId)
        {
            return await _context.UserTenants
                .AnyAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
        }

        public async Task<UserTenant> GetByUserAndTenantIdAsync(Guid userId, Guid tenantId)
        {
            return await _context.UserTenants
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
        }
    }
}
