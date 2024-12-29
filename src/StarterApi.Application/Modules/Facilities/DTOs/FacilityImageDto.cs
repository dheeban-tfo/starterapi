using System;
using Microsoft.AspNetCore.Http;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityImageDto
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }
        public Guid DocumentId { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        
        // Document properties
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string Description { get; set; }
        public string DownloadUrl { get; set; }
    }

    public class CreateFacilityImageDto
    {
        public Guid FacilityId { get; set; }
        public IFormFile File { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
    }

    public class UpdateFacilityImageDto
    {
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }  // Optional, only if updating the image
    }
} 