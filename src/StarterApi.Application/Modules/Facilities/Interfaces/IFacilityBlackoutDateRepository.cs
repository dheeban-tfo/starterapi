using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBlackoutDateRepository
    {
        Task<PagedResult<FacilityBlackoutDate>> GetPagedAsync(Guid facilityId, QueryParameters parameters);
        Task<FacilityBlackoutDate> GetByIdAsync(Guid id);
        Task<IEnumerable<FacilityBlackoutDate>> GetByDateRangeAsync(Guid facilityId, DateTime startDate, DateTime endDate);
        Task<FacilityBlackoutDate> AddAsync(FacilityBlackoutDate blackoutDate);
        Task<FacilityBlackoutDate> UpdateAsync(FacilityBlackoutDate blackoutDate);
        Task<bool> DeleteAsync(FacilityBlackoutDate blackoutDate);
        Task SaveChangesAsync();
    }
} 