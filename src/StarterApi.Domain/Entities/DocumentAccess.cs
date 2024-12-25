using System;

namespace StarterApi.Domain.Entities
{
    public class DocumentAccess : BaseEntity
    {
        public Guid DocumentId { get; set; }
        public Guid UserId { get; set; }
        public string AccessLevel { get; set; } // Read, Write, Admin
        
        // Navigation properties
        public virtual Document Document { get; set; }
        public virtual TenantUser User { get; set; }
    }
}
