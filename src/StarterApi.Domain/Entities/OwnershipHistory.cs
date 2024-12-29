using System;
using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class OwnershipHistory : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid OwnerId { get; set; }
        public string TransferType { get; set; }  // Purchase, Inheritance, Gift
        public DateTime TransferDate { get; set; }
        public string TransferReason { get; set; }
        public string PreviousOwnerName { get; set; }
        public string TransferDocumentNumber { get; set; }
        public string Status { get; set; }  // Completed, Reverted
        
        // Navigation properties
        public virtual Unit Unit { get; set; }
        public virtual Owner Owner { get; set; }
        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();
    }
} 