using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Common;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class ParkingSlotConfiguration : BaseConfiguration<ParkingSlot>
    {
        public override void Configure(EntityTypeBuilder<ParkingSlot> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SlotNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Location)
                .HasMaxLength(200);

            // Relationships
            builder.HasOne(e => e.Society)
                .WithMany()
                .HasForeignKey(e => e.SocietyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Allocations)
                .WithOne(a => a.ParkingSlot)
                .HasForeignKey(a => a.ParkingSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 