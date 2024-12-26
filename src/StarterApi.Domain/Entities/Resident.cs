namespace StarterApi.Domain.Entities
{
    public class Resident : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid IndividualId { get; set; }
        public Guid? UserId { get; set; }
        public string ResidentType { get; set; } // Owner, Family, Tenant
        public string RelationToOwner { get; set; }
        public bool PrimaryResident { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public Guid? VerifiedBy { get; set; }

        // Navigation properties
        public virtual Unit Unit { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual TenantUser User { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
    }
} 