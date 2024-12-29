using System;
using System.Collections.Generic;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;

namespace StarterApi.Application.Modules.Owners.DTOs
{
    // List DTO
    public class OwnershipHistoryListDto
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string OwnerName { get; set; }
        public string TransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public string Status { get; set; }
        public string PreviousOwnerName { get; set; }
    }

    // Detail DTO
    public class OwnershipHistoryDto
    {
        public Guid Id { get; set; }
        public LookupDetailDto SelectedUnit { get; set; }
        public LookupDetailDto SelectedOwner { get; set; }
        public string TransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public string TransferReason { get; set; }
        public string PreviousOwnerName { get; set; }
        public string TransferDocumentNumber { get; set; }
        public string Status { get; set; }
        public List<DocumentDto> Documents { get; set; }
        public AuditDto Audit { get; set; }
    }
} 