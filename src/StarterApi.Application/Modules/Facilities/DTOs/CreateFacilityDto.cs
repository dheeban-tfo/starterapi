using System.ComponentModel.DataAnnotations;
using StarterApi.Domain.Enums;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class CreateFacilityDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Capacity { get; set; }

        [Required]
        public FacilityType Type { get; set; }

        public bool IsChargeable { get; set; }

        [Range(0, 10000)]
        public decimal? ChargePerHour { get; set; }

        [Required]
        [StringLength(100)]
        public string OperatingHours { get; set; }

        [StringLength(500)]
        public string MaintenanceSchedule { get; set; }

        [StringLength(1000)]
        public string Rules { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool RequiresBooking { get; set; }

        [Range(0, 168)]
        public int? MinimumNoticeHours { get; set; }

        [Range(0, 365)]
        public int? MaximumBookingDays { get; set; }

        public bool AllowMultipleBookings { get; set; }

        [Required]
        public Guid SocietyId { get; set; }
    }
} 