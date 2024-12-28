using StarterApi.Domain.Enums;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public FacilityType Type { get; set; }
        public FacilityStatus Status { get; set; }
        public bool IsChargeable { get; set; }
        public decimal? ChargePerHour { get; set; }
        public string OperatingHours { get; set; }
        public bool RequiresBooking { get; set; }
        public string ImageUrl { get; set; }
        public string SocietyName { get; set; }
        public int ActiveBookingsCount { get; set; }
    }
} 