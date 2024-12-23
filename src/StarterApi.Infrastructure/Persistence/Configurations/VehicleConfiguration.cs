using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class VehicleConfiguration : BaseConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            base.Configure(builder);

            builder.ToTable("Vehicles");

            builder.Property(v => v.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(v => v.Make)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(v => v.Resident)
                .WithMany(r => r.Vehicles)
                .HasForeignKey(v => v.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 