 namespace StarterApi.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Guid ResidentId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string VehicleType { get; set; }
        public bool IsElectric { get; set; }
        public bool ChargingSlotRequired { get; set; }

        public virtual Resident Resident { get; set; }
        private readonly List<ParkingAllocation> _parkingAllocations = new();
        public IReadOnlyCollection<ParkingAllocation> ParkingAllocations => _parkingAllocations.AsReadOnly();
    }
} 