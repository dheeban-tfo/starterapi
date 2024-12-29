using System;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityBookingListDto
    {
        public Guid Id { get; set; }
        public string FacilityName { get; set; }
        public string ResidentName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string BookingStatus { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
} 