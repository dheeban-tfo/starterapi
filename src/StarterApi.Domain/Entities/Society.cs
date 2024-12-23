 namespace StarterApi.Domain.Entities
{
    public class Society : BaseEntity
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int TotalBlocks { get; set; }
        public string RegistrationNumber { get; set; }
        
        private readonly List<Block> _blocks = new();
        public IReadOnlyCollection<Block> Blocks => _blocks.AsReadOnly();
    }
} 