using StarterApi.Application.Common.Models;
using StarterApi.Domain.Enums;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public FacilityType Type { get; set; }
        public FacilityStatus Status { get; set; }
        public bool IsChargeable { get; set; }
        public decimal? ChargePerHour { get; set; }
        public string OperatingHours { get; set; }
        public string MaintenanceSchedule { get; set; }
        public string Rules { get; set; }
        public string ImageUrl { get; set; }
        public bool RequiresBooking { get; set; }
        public int? MinimumNoticeHours { get; set; }
        public int? MaximumBookingDays { get; set; }
        public bool AllowMultipleBookings { get; set; }
        public LookupDetailDto SelectedSociety { get; set; }
        public int ActiveBookingsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
} 