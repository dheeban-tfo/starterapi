using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class UpdateFacilityBookingRuleDto
    {
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format. Use HH:mm")]
        public string StartTime { get; set; }

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Invalid time format. Use HH:mm")]
        public string EndTime { get; set; }

        [Required]
        [Range(15, 1440)]
        public int MaxDurationMinutes { get; set; }

        [Required]
        [Range(15, 1440)]
        public int MinDurationMinutes { get; set; }

        [Required]
        [Range(0, 168)]
        public int MinAdvanceBookingHours { get; set; }

        [Required]
        [Range(1, 365)]
        public int MaxAdvanceBookingDays { get; set; }

        public bool AllowMultipleBookings { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxBookingsPerDay { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxActiveBookings { get; set; }

        [Range(0, 10000)]
        public decimal? PricePerHour { get; set; }

        public bool RequireApproval { get; set; }

        [StringLength(1000)]
        public string CancellationPolicy { get; set; }
    }
} 