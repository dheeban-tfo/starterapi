namespace StarterApi.Domain.Entities
{
    public class FacilityBookingRule : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public virtual Facility Facility { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
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
    }
} 