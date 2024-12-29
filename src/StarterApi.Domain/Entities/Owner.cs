using System;
using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class Owner : BaseEntity
    {
        public Guid IndividualId { get; set; }
        public string OwnershipType { get; set; }  // Primary, Joint, Corporate
        public decimal OwnershipPercentage { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public DateTime? OwnershipEndDate { get; set; }
        public string Status { get; set; }  // Active, Pending, Transferred
        public string OwnershipDocumentNumber { get; set; }

        // Navigation properties
        public virtual Individual Individual { get; set; }
        private readonly List<Unit> _units = new();
        public IReadOnlyCollection<Unit> Units => _units.AsReadOnly();
        private readonly List<OwnershipHistory> _ownershipHistory = new();
        public IReadOnlyCollection<OwnershipHistory> OwnershipHistory => _ownershipHistory.AsReadOnly();
        private readonly List<Document> _documents = new();
        public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();
    }
} 