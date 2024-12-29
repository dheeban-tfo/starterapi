using System;
using StarterApi.Domain.Common;

namespace StarterApi.Domain.Entities
{
    public class FacilityImage : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Guid DocumentId { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        
        // Navigation properties
        public virtual Facility Facility { get; set; }
        public virtual Document Document { get; set; }
    }
} 