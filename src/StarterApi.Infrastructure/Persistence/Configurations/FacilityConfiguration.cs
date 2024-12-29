using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class FacilityConfiguration : BaseConfiguration<Facility>
    {
        public override void Configure(EntityTypeBuilder<Facility> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Location)
                .HasMaxLength(200);

            builder.Property(e => e.Capacity)
                .IsRequired();

            builder.Property(e => e.ChargePerHour)
                .HasPrecision(18, 2);

            // Relationships
            builder.HasOne(e => e.Society)
                .WithMany()
                .HasForeignKey(e => e.SocietyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.BookingRule)
                .WithOne(r => r.Facility)
                .HasForeignKey<FacilityBookingRule>(r => r.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.BlackoutDates)
                .WithOne(b => b.Facility)
                .HasForeignKey(b => b.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Images)
                .WithOne(i => i.Facility)
                .HasForeignKey(i => i.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 