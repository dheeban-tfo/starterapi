using System;
using System.Collections.Generic;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;

namespace StarterApi.Application.Modules.Owners.DTOs
{
    // List DTO for collection endpoints
    public class OwnerListDto
    {
        public Guid Id { get; set; }
        public string OwnerName { get; set; }
        public string OwnershipType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public string Status { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public int UnitCount { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }

    // Detail DTO for single item endpoints
    public class OwnerDto
    {
        public Guid Id { get; set; }
        public LookupDetailDto SelectedIndividual { get; set; }
        public string OwnershipType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public DateTime? OwnershipEndDate { get; set; }
        public string Status { get; set; }
        public string OwnershipDocumentNumber { get; set; }
        public List<LookupDetailDto> Units { get; set; }
        public List<OwnershipHistoryDto> OwnershipHistory { get; set; }
        public List<DocumentDto> Documents { get; set; }
        public AuditDto Audit { get; set; }
    }

    // Create DTO
    public class CreateOwnerDto
    {
        public Guid IndividualId { get; set; }
        public string OwnershipType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public DateTime OwnershipStartDate { get; set; }
        public string OwnershipDocumentNumber { get; set; }
        public List<Guid> UnitIds { get; set; }
        public List<Guid> DocumentIds { get; set; }
    }

    // Update DTO
    public class UpdateOwnerDto
    {
        public string OwnershipType { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public DateTime? OwnershipEndDate { get; set; }
        public string Status { get; set; }
        public string OwnershipDocumentNumber { get; set; }
        public List<Guid> UnitIds { get; set; }
    }
} 