using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBookingRuleRepository
    {
        Task<FacilityBookingRule> GetByFacilityIdAsync(Guid facilityId);
        Task<FacilityBookingRule> AddAsync(FacilityBookingRule rule);
        Task<FacilityBookingRule> UpdateAsync(FacilityBookingRule rule);
        Task SaveChangesAsync();
    }
} 