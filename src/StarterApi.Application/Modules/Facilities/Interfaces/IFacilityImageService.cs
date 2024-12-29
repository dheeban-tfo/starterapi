using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Modules.Facilities.DTOs;

namespace StarterApi.Application.Modules.Facilities.Interfaces
{
    public interface IFacilityImageService
    {
        Task<IEnumerable<FacilityImageDto>> GetByFacilityIdAsync(Guid facilityId);
        Task<FacilityImageDto> UploadImageAsync(CreateFacilityImageDto dto);
        Task<FacilityImageDto> UpdateImageAsync(Guid id, UpdateFacilityImageDto dto);
        Task<bool> DeleteImageAsync(Guid id);
        Task<ImageContentDto> GetImageContentAsync(Guid id);
    }

    public class ImageContentDto
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
} 