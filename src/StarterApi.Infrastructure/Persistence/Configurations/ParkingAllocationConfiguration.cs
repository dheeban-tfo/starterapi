using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class ParkingAllocationConfiguration : BaseConfiguration<ParkingAllocation>
    {
        public override void Configure(EntityTypeBuilder<ParkingAllocation> builder)
        {
            base.Configure(builder);

            builder.ToTable("ParkingAllocations");
            
            builder.HasKey(pa => pa.Id);

            builder.HasOne(pa => pa.ParkingSlot)
                .WithMany(ps => ps.Allocations)
                .HasForeignKey(pa => pa.ParkingSlotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.Vehicle)
                .WithMany(v => v.ParkingAllocations)
                .HasForeignKey(pa => pa.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.Resident)
                .WithMany()
                .HasForeignKey(pa => pa.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 