 namespace StarterApi.Domain.Entities
{
    public class Complaint : BaseEntity
    {
        public Guid ResidentId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime RaisedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid? AssignedToId { get; set; }
        public string ResolutionDetails { get; set; }

        public virtual Resident Resident { get; set; }
        public virtual Resident AssignedTo { get; set; }
    }
} 