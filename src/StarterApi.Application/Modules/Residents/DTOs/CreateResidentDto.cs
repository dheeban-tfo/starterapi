using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StarterApi.Application.Modules.Individuals.DTOs;

namespace StarterApi.Application.Modules.Residents.DTOs
{
    public class CreateResidentDto
    {
        [Required]
        public Guid UnitId { get; set; }

        [Required]
        public CreateIndividualDto Individual { get; set; }

        [Required]
        [StringLength(20)]
        public string ResidentType { get; set; }

        [Required]
        [StringLength(50)]
        public string RelationToOwner { get; set; }

        public bool PrimaryResident { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; } = DateTime.UtcNow;

        public DateTime? CheckOutDate { get; set; }

        public ICollection<Guid> DocumentIds { get; set; } = new List<Guid>();
    }
} 