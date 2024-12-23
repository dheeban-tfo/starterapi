 namespace StarterApi.Domain.Entities
{
    public class Block : BaseEntity
    {
        public Guid SocietyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TotalFloors { get; set; }
        public decimal MaintenanceChargePerSqft { get; set; }
        
        public virtual Society Society { get; set; }
        private readonly List<Floor> _floors = new();
        public IReadOnlyCollection<Floor> Floors => _floors.AsReadOnly();
    }
} 