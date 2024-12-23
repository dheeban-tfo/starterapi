 namespace StarterApi.Domain.Entities
{
    public class Owner : BaseEntity
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string AlternateContactNumber { get; set; }
        public string Address { get; set; }
        public string IDProofType { get; set; }
        public string IDProofNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }

        private readonly List<Unit> _units = new();
        public IReadOnlyCollection<Unit> Units => _units.AsReadOnly();
    }
} 