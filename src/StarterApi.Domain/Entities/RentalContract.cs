 namespace StarterApi.Domain.Entities
{
    public class RentalContract : BaseEntity
    {
        public Guid UnitId { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal RentAmount { get; set; }
        public decimal SecurityDeposit { get; set; }
        public string PaymentFrequency { get; set; }
        public string PaymentMode { get; set; }
        public string Terms { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual Resident Tenant { get; set; }
    }
} 