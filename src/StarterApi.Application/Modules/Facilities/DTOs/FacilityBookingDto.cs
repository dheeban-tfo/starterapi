using System;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Modules.Facilities.DTOs
{
    public class FacilityBookingDto
    {
        public Guid Id { get; set; }
        public FacilityLookupDto SelectedFacility { get; set; }
        public ResidentLookupDto SelectedResident { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string BookingStatus { get; set; }
        public string SpecialRequest { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
} 