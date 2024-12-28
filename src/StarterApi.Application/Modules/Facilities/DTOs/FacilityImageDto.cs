using System;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityImageDto
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateFacilityImageDto
    {
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateFacilityImageDto
    {
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
} 