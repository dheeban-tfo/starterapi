using System;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class CreateFacilityBookingDto
    {
        [Required]
        public Guid FacilityId { get; set; }

        [Required]
        public Guid ResidentId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public string SpecialRequest { get; set; }
    }
} 