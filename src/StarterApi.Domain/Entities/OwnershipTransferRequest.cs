using System;
using System.Collections.Generic;

namespace StarterApi.Domain.Entities
{
    public class OwnershipTransferRequest : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid CurrentOwnerId { get; set; }
        public Guid NewOwnerId { get; set; }
        public string TransferType { get; set; }  // Purchase, Inheritance, Gift
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }  // Pending, Approved, Rejected
        public string Comments { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RejectionReason { get; set; }
        
        // Navigation properties
        public virtual Unit Unit { get; set; }
        public virtual Owner CurrentOwner { get; set; }
        public virtual Owner NewOwner { get; set; }
        public virtual TenantUser ApprovedByUser { get; set; }
        private readonly List<Document> _supportingDocuments = new();
        public IReadOnlyCollection<Document> SupportingDocuments => _supportingDocuments.AsReadOnly();
    }
} 