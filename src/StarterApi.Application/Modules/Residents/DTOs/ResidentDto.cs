using StarterApi.Application.Common.Models;
using System;

namespace StarterApi.Application.Modules.Residents.DTOs
{
    public class ResidentDto
    {
        public Guid Id { get; set; }
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

        public IndividualDetailDto SelectedIndividual { get; set; }
        public UnitDetailDto SelectedUnit { get; set; }
    }
} 