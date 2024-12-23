using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class FacilityBookingConfiguration : BaseConfiguration<FacilityBooking>
    {
        public override void Configure(EntityTypeBuilder<FacilityBooking> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("FacilityBookings");
            
            builder.HasKey(fb => fb.Id);
            
            builder.Property(fb => fb.BookingStatus)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(fb => fb.SpecialRequest)
                .HasMaxLength(500);
                
            builder.Property(fb => fb.ChargeAmount)
                .HasPrecision(18, 2);
                
            builder.Property(fb => fb.PaymentStatus)
                .HasMaxLength(20);

            builder.HasOne(fb => fb.Facility)
                .WithMany(f => f.Bookings)
                .HasForeignKey(fb => fb.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fb => fb.Resident)
                .WithMany()
                .HasForeignKey(fb => fb.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 