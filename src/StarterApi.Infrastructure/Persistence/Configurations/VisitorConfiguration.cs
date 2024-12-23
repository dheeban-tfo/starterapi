using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class VisitorConfiguration : BaseConfiguration<Visitor>
    {
        public override void Configure(EntityTypeBuilder<Visitor> builder)
        {
            base.Configure(builder);

            builder.ToTable("Visitors");
            
            builder.HasKey(v => v.Id);
            
            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(v => v.ContactNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(v => v.VehicleRegistrationNumber)
                .HasMaxLength(20);
                
            builder.Property(v => v.Purpose)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(v => v.Remarks)
                .HasMaxLength(500);

            builder.HasOne(v => v.RegisteredBy)
                .WithMany()
                .HasForeignKey(v => v.RegisteredById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.VisitedUnit)
                .WithMany()
                .HasForeignKey(v => v.VisitedUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 