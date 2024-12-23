 namespace StarterApi.Domain.Entities
{
    public class FacilityBooking : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public Guid ResidentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string BookingStatus { get; set; }
        public string SpecialRequest { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string PaymentStatus { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual Resident Resident { get; set; }
    }
} 