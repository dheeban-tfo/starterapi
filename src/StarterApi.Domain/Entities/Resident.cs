namespace StarterApi.Domain.Entities
{
    public class Resident : BaseEntity
    {
        public Guid UnitId { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RelationToOwner { get; set; }
        public bool PrimaryResident { get; set; }
        public string ProfilePhoto { get; set; }
        public string IDProofType { get; set; }
        public string IDProofNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
} 