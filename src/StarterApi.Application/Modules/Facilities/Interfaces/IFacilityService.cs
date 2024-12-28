using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityService
    {
        Task<PagedResult<FacilityListDto>> GetFacilitiesAsync(QueryParameters parameters);
        Task<FacilityDto> GetByIdAsync(Guid id);
        Task<FacilityDto> CreateAsync(CreateFacilityDto dto);
        Task<FacilityDto> UpdateAsync(Guid id, UpdateFacilityDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<FacilityTypeDto>> GetFacilityTypesAsync();
        Task<IEnumerable<FacilityStatusDto>> GetFacilityStatusTypesAsync();
        Task<bool> TemporaryCloseAsync(Guid id);
        Task<bool> ReopenAsync(Guid id);
        Task<IEnumerable<FacilityListDto>> GetBySocietyIdAsync(Guid societyId);
    }
} 