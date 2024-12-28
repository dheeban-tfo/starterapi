namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityBookingRuleDto
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int MaxDurationMinutes { get; set; }
        public int MinDurationMinutes { get; set; }
        public int MinAdvanceBookingHours { get; set; }
        public int MaxAdvanceBookingDays { get; set; }
        public bool AllowMultipleBookings { get; set; }
        public int MaxBookingsPerDay { get; set; }
        public int MaxActiveBookings { get; set; }
        public decimal? PricePerHour { get; set; }
        public bool RequireApproval { get; set; }
        public string CancellationPolicy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
} 