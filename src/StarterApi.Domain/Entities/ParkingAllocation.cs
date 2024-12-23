 namespace StarterApi.Domain.Entities
{
    public class ParkingAllocation : BaseEntity
    {
        public Guid ParkingSlotId { get; set; }
        public Guid ResidentId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ParkingSlot ParkingSlot { get; set; }
        public virtual Resident Resident { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
} 