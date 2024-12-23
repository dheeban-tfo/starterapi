 namespace StarterApi.Domain.Entities
{
    public class Visitor : BaseEntity
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string Purpose { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public Guid VisitedUnitId { get; set; }
        public Guid RegisteredById { get; set; }
        public string Remarks { get; set; }

        public virtual Unit VisitedUnit { get; set; }
        public virtual Resident RegisteredBy { get; set; }
    }
} 