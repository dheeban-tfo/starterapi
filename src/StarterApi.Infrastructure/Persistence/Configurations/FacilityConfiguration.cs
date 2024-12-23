using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class FacilityConfiguration : BaseConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("Facilities");
            
            builder.HasKey(f => f.Id);
            
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(f => f.Description)
                .HasMaxLength(500);
                
            builder.Property(f => f.Location)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(f => f.OperatingHours)
                .HasMaxLength(200);
                
            builder.Property(f => f.MaintenanceSchedule)
                .HasMaxLength(200);
                
            builder.Property(f => f.ChargePerHour)
                .HasPrecision(18, 2);

            builder.HasMany(f => f.Bookings)
                .WithOne(b => b.Facility)
                .HasForeignKey(b => b.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 