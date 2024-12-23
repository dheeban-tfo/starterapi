 namespace StarterApi.Domain.Entities
{
    public class Floor : BaseEntity
    {
        public Guid BlockId { get; set; }
        public int FloorNumber { get; set; }
        public string FloorName { get; set; }
        public int TotalUnits { get; set; }
        
        public virtual Block Block { get; set; }
        private readonly List<Unit> _units = new();
        public IReadOnlyCollection<Unit> Units => _units.AsReadOnly();
    }
} 