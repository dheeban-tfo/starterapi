namespace StarterApi.Domain.Entities
{
    public class Owner : BaseEntity
    {
        public Guid IndividualId { get; set; }
        public string OwnershipType { get; set; }
        public DateTime? OwnershipStartDate { get; set; }
        public DateTime? OwnershipEndDate { get; set; }

        // Navigation properties
        public virtual Individual Individual { get; set; }
        public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
    }
} 