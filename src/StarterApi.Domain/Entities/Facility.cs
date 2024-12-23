 namespace StarterApi.Domain.Entities
{
    public class Facility : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public bool IsChargeable { get; set; }
        public decimal? ChargePerHour { get; set; }
        public string OperatingHours { get; set; }
        public string MaintenanceSchedule { get; set; }

        private readonly List<FacilityBooking> _bookings = new();
        public IReadOnlyCollection<FacilityBooking> Bookings => _bookings.AsReadOnly();
    }
} 