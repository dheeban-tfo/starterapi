namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class TimeSlotDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public string UnavailabilityReason { get; set; }
    }
} 