using System;

namespace StarterApi.Domain.Entities
{
    public class OwnerDocument
    {
        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }
    }
} 