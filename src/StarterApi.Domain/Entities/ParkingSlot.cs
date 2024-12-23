 namespace StarterApi.Domain.Entities
{
    public class ParkingSlot : BaseEntity
    {
        public Guid SocietyId { get; set; }
        public string SlotNumber { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public virtual Society Society { get; set; }
        private readonly List<ParkingAllocation> _allocations = new();
        public IReadOnlyCollection<ParkingAllocation> Allocations => _allocations.AsReadOnly();
    }
} 