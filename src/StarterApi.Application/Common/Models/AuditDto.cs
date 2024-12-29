using System;

namespace StarterApi.Application.Common.Models
{
    public class AuditDto
    {
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
} 