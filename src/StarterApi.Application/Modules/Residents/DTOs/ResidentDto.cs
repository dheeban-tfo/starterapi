using System;
using System.Collections.Generic;
using StarterApi.Application.Modules.Individuals.DTOs;
using StarterApi.Application.Modules.Units.DTOs;

namespace StarterApi.Application.Modules.Residents.DTOs
{
    public class ResidentDto
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public Guid IndividualId { get; set; }
        public Guid? UserId { get; set; }
        public string ResidentType { get; set; }
        public string RelationToOwner { get; set; }
        public bool PrimaryResident { get; set; }
        public string Status { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public Guid? VerifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public IndividualDto Individual { get; set; }
        public UnitDto Unit { get; set; }
    }
} 