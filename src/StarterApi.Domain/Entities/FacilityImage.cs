using System;

namespace StarterApi.Domain.Entities
{
    public class FacilityImage : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public string Description { get; set; }

        // Navigation property
        public virtual Facility Facility { get; set; }
    }
} 