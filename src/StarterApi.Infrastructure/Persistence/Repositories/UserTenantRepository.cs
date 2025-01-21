using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class UserTenantRepository : IUserTenantRepository
    {
        private readonly IRootDbContext _rootContext;
        private readonly ITenantDbContext _tenantContext;
        private readonly ILogger<UserTenantRepository> _logger;

        public UserTenantRepository(
            IRootDbContext rootContext,
            ITenantDbContext tenantContext,
            ILogger<UserTenantRepository> logger)
        {
            _rootContext = rootContext;
            _tenantContext = tenantContext;
            _logger = logger;
        }

        public async Task<UserTenant> GetByMobileNumberAsync(string mobileNumber)
        {
            _logger.LogInformation("Looking up UserTenant by mobile number: {MobileNumber}", mobileNumber);

            var userTenant = await _rootContext.UserTenants
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.User.MobileNumber == mobileNumber);

            _logger.LogInformation("UserTenant lookup result: {Found}", userTenant != null);
            return userTenant;
        }

        public async Task<UserTenant> AddAsync(UserTenant userTenant)
        {
            await _rootContext.UserTenants.AddAsync(userTenant);
            return userTenant;
        }

        public async Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId)
        {
            return await _rootContext.UserTenants
                .Include(ut => ut.Tenant)
                .Where(ut => ut.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTenant>> GetByTenantIdAsync(Guid tenantId)
        {
            return await _rootContext.UserTenants
                .Include(ut => ut.User)
                .Where(ut => ut.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _rootContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid tenantId)
        {
            return await _rootContext.UserTenants
                .AnyAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
        }

        public async Task<UserTenant> GetByUserAndTenantIdAsync(Guid userId, Guid tenantId)
        {
            return await _rootContext.UserTenants
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);
        }

        public async Task<bool> UserHasPermissionAsync(Guid userId, Guid tenantId, string permission)
        {
            return await _rootContext.UserTenants
                .Where(ut => ut.UserId == userId && ut.TenantId == tenantId)
                .Join(_rootContext.RolePermissions,
                    ut => ut.RoleId,
                    rp => rp.RoleId,
                    (ut, rp) => rp.Permission)
                .Join(_rootContext.Permissions,
                    rp => rp.Id,
                    p => p.Id,
                    (rp, p) => p.SystemName)
                .AnyAsync(p => p == permission);
        }

        public async Task<List<Role>> GetTenantRolesAsync(Guid tenantId)
        {
            return await _rootContext.Roles
                .Where(r => r.TenantId == tenantId)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetTenantPermissionsAsync(Guid tenantId)
        {
            return await _rootContext.Permissions
                .Where(p => _rootContext.RolePermissions
                    .Any(rp => rp.Permission.Id == p.Id && 
                              rp.Role.TenantId == tenantId))
                .ToListAsync();
        }

        public async Task<List<string>> GetUserPermissionsAsync(Guid userId, Guid tenantId)
        {
            // First check if this is a valid user-tenant relationship in root DB
            var userTenant = await _rootContext.UserTenants
                .AsNoTracking()
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TenantId == tenantId);

            if (userTenant == null)
            {
                _logger.LogWarning("No user-tenant relationship found for user {UserId} and tenant {TenantId}", userId, tenantId);
                return new List<string>();
            }

            // Get permissions from tenant DB
            return await _tenantContext.Users
                .Where(u => u.Id == userId)
                .Join(_tenantContext.RolePermissions,
                    u => u.RoleId,
                    rp => rp.RoleId,
                    (u, rp) => rp.Permission)
                .Join(_tenantContext.Permissions,
                    rp => rp.Id,
                    p => p.Id,
                    (rp, p) => p.SystemName)
                .Distinct()
                .ToListAsync();
        }
    }
}
