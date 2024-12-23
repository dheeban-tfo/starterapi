using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class ParkingSlotConfiguration : BaseConfiguration<ParkingSlot>
    {
        public void Configure(EntityTypeBuilder<ParkingSlot> builder)
        {
            builder.ToTable("ParkingSlots");
            
            builder.HasKey(ps => ps.Id);
            
            builder.Property(ps => ps.SlotNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(ps => ps.Type)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(ps => ps.Location)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(ps => ps.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(ps => ps.Society)
                .WithMany()
                .HasForeignKey(ps => ps.SocietyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ps => ps.Allocations)
                .WithOne(pa => pa.ParkingSlot)
                .HasForeignKey(pa => pa.ParkingSlotId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 