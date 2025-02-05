using System;
using System.Collections.Generic;
using StarterApi.Domain.Enums;

namespace StarterApi.Domain.Entities
{
    public class Facility : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public FacilityType Type { get; set; }
        public FacilityStatus Status { get; set; }
        public bool IsChargeable { get; set; }
        public decimal? ChargePerHour { get; set; }
        public string OperatingHours { get; set; }
        public string MaintenanceSchedule { get; set; }
        public string Rules { get; set; }
        public string ImageUrl { get; set; }
        public bool RequiresBooking { get; set; }
        public int? MinimumNoticeHours { get; set; }
        public int? MaximumBookingDays { get; set; }
        public bool AllowMultipleBookings { get; set; }
        public Guid? SocietyId { get; set; }
        public virtual Society Society { get; set; }



        // Navigation properties
        public virtual ICollection<FacilityBooking> Bookings { get; set; }
        public virtual FacilityBookingRule BookingRule { get; set; }
        public virtual ICollection<FacilityBlackoutDate> BlackoutDates { get; set; }
        public virtual ICollection<FacilityImage> Images { get; set; }

        public Facility()
        {
            Bookings = new HashSet<FacilityBooking>();
            BlackoutDates = new HashSet<FacilityBlackoutDate>();
            Images = new HashSet<FacilityImage>();
        }
    }
} 