using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityImageService
    {
        Task<IEnumerable<FacilityImageDto>> GetByFacilityIdAsync(Guid facilityId);
        Task<FacilityImageDto> GetByIdAsync(Guid id);
        Task<FacilityImageDto> UploadAsync(Guid facilityId, IFormFile file, CreateFacilityImageDto dto);
        Task<FacilityImageDto> UpdateAsync(Guid id, UpdateFacilityImageDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<FacilityImageDto> SetPrimaryImageAsync(Guid facilityId, Guid imageId);
        Task<bool> ReorderImagesAsync(Guid facilityId, IEnumerable<Guid> imageIds);
    }
} 