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

            builder.Property(e => e.BookingStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Date)
                .IsRequired();

            builder.Property(e => e.StartTime)
                .IsRequired();

            builder.Property(e => e.EndTime)
                .IsRequired();

            // Relationships
            builder.HasOne(e => e.Facility)
                .WithMany(f => f.Bookings)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Resident)
                .WithMany()
                .HasForeignKey(e => e.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.FacilityId, e.Date, e.StartTime, e.EndTime })
                .HasDatabaseName("IX_FacilityBooking_Availability");
        }
    }
} 