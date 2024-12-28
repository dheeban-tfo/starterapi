namespace StarterApi.Domain.Entities
{
    public class FacilityBlackoutDate : BaseEntity
    {
        public Guid FacilityId { get; set; }
        public virtual Facility Facility { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
    }
} 