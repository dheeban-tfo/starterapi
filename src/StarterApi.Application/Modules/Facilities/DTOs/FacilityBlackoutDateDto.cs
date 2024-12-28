namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityBlackoutDateDto
    {
        public Guid Id { get; set; }
        public Guid FacilityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
} 