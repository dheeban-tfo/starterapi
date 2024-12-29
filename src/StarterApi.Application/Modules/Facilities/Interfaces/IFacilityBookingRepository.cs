using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBookingRepository
    {
        Task<FacilityBooking> GetByIdAsync(Guid id);
        Task<PagedResult<FacilityBooking>> GetPagedAsync(QueryParameters parameters);
        Task<PagedResult<FacilityBooking>> GetByFacilityAsync(Guid facilityId, QueryParameters parameters);
        Task<PagedResult<FacilityBooking>> GetByResidentAsync(Guid residentId, QueryParameters parameters);
        Task<bool> HasOverlappingBookingsAsync(Guid facilityId, DateTime date, TimeSpan startTime, TimeSpan endTime, Guid? excludeBookingId = null);
        Task<bool> ExistsActiveBookingAsync(Guid id);
        Task<FacilityBooking> AddAsync(FacilityBooking booking);
        Task<FacilityBooking> UpdateAsync(FacilityBooking booking);
        Task<bool> DeleteAsync(FacilityBooking booking);
        Task SaveChangesAsync();
    }
} 