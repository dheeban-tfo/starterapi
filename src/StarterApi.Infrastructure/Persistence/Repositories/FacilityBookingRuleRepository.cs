using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FacilityBookingRuleRepository : IFacilityBookingRuleRepository
    {
        private readonly ITenantDbContext _context;

        public FacilityBookingRuleRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<FacilityBookingRule> GetByFacilityIdAsync(Guid facilityId)
        {
            return await _context.FacilityBookingRules
                .FirstOrDefaultAsync(r => r.FacilityId == facilityId && r.IsActive);
        }

        public async Task<FacilityBookingRule> AddAsync(FacilityBookingRule rule)
        {
            await _context.FacilityBookingRules.AddAsync(rule);
            await SaveChangesAsync();
            return rule;
        }

        public async Task<FacilityBookingRule> UpdateAsync(FacilityBookingRule rule)
        {
            _context.FacilityBookingRules.Update(rule);
            await SaveChangesAsync();
            return rule;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 