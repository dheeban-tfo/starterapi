using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StarterApi.Application.Modules.Individuals.DTOs;

namespace StarterApi.Application.Modules.Residents.DTOs
{
    public class UpdateResidentDto
    {
        [Required]
        [StringLength(20)]
        public string ResidentType { get; set; }

        [Required]
        [StringLength(50)]
        public string RelationToOwner { get; set; }

        public bool PrimaryResident { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public string Status { get; set; }

        public UpdateIndividualDto Individual { get; set; }

        public ICollection<Guid> DocumentIds { get; set; } = new List<Guid>();
    }
} 