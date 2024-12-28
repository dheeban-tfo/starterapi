using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class UpdateFacilityBlackoutDateDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        public bool IsRecurring { get; set; }

        [StringLength(100)]
        public string RecurrencePattern { get; set; }
    }
} 