using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBookingRuleService
    {
        Task<FacilityBookingRuleDto> GetByFacilityIdAsync(Guid facilityId);
        Task<FacilityBookingRuleDto> UpdateAsync(Guid facilityId, UpdateFacilityBookingRuleDto dto);
        Task<IEnumerable<TimeSlotDto>> GetAvailableSlotsAsync(Guid facilityId, DateTime date);
    }
} 