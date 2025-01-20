using System;
using StarterApi.Domain.Common;

namespace StarterApi.Domain.Entities
{
    public class Visitor : BaseEntity
    {
        public string VisitorName { get; set; }
        public DateTime ExpectedVisitDate { get; set; }
        public TimeSpan ExpectedVisitStartTime { get; set; }
        public TimeSpan ExpectedVisitEndTime { get; set; }
        public string PurposeOfVisit { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected, etc.
        public bool IsParking { get; set; } // Indicates if parking is required
        
        // Navigation properties
        public Guid? ResidentId { get; set; }
        public virtual Individual Resident { get; set; }
    }
} 