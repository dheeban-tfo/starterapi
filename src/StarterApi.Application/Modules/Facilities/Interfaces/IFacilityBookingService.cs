using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityBookingService
    {
        Task<PagedResult<FacilityBookingListDto>> GetBookingsAsync(QueryParameters parameters);
        Task<FacilityBookingDto> GetByIdAsync(Guid id);
        Task<FacilityBookingDto> CreateBookingAsync(CreateFacilityBookingDto dto);
        Task<bool> CancelBookingAsync(Guid id);
        Task<bool> CheckAvailabilityAsync(Guid facilityId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<PagedResult<FacilityBookingListDto>> GetBookingsByFacilityAsync(Guid facilityId, QueryParameters parameters);
        Task<PagedResult<FacilityBookingListDto>> GetBookingsByResidentAsync(Guid residentId, QueryParameters parameters);
    }
} 