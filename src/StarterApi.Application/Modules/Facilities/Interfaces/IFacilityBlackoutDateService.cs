using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBlackoutDateService
    {
        Task<PagedResult<FacilityBlackoutDateDto>> GetPagedAsync(Guid facilityId, QueryParameters parameters);
        Task<FacilityBlackoutDateDto> GetByIdAsync(Guid id);
        Task<IEnumerable<FacilityBlackoutDateDto>> GetByDateRangeAsync(Guid facilityId, DateTime startDate, DateTime endDate);
        Task<FacilityBlackoutDateDto> UpdateAsync(Guid id, UpdateFacilityBlackoutDateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
} 