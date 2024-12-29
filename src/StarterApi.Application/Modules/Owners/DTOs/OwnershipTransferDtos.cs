using System;
using System.Collections.Generic;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;

namespace StarterApi.Application.Modules.Owners.DTOs
{
    // List DTO
    public class OwnershipTransferListDto
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string CurrentOwnerName { get; set; }
        public string NewOwnerName { get; set; }
        public string TransferType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }

    // Detail DTO
    public class OwnershipTransferDto
    {
        public Guid Id { get; set; }
        public LookupDetailDto SelectedUnit { get; set; }
        public LookupDetailDto SelectedCurrentOwner { get; set; }
        public LookupDetailDto SelectedNewOwner { get; set; }
        public string TransferType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public LookupDetailDto ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RejectionReason { get; set; }
        public List<DocumentDto> SupportingDocuments { get; set; }
        public AuditDto Audit { get; set; }
    }

    // Create DTO
    public class CreateOwnershipTransferDto
    {
        public Guid UnitId { get; set; }
        public Guid NewOwnerId { get; set; }
        public string TransferType { get; set; }
        public string Comments { get; set; }
        public List<Guid> DocumentIds { get; set; }
    }

    // Update DTO for Approval/Rejection
    public class UpdateOwnershipTransferStatusDto
    {
        public string Status { get; set; }
        public string RejectionReason { get; set; }
    }
} 