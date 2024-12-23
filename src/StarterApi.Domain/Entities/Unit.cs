namespace StarterApi.Domain.Entities
{
    public class Unit : BaseEntity
    {
        public Guid FloorId { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public Guid? CurrentOwnerId { get; set; }
        public decimal MonthlyMaintenanceFee { get; set; }
        
        public virtual Floor Floor { get; set; }
        public virtual Owner CurrentOwner { get; set; }
        public virtual ICollection<Resident> Residents { get; set; } = new List<Resident>();
    }
} 