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

            builder.Property(e => e.VisitorName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ExpectedVisitDate)
                .IsRequired();

            builder.Property(e => e.ExpectedVisitStartTime)
                .IsRequired();

            builder.Property(e => e.ExpectedVisitEndTime)
                .IsRequired();

            builder.Property(e => e.PurposeOfVisit)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.IsParking)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(e => e.Resident)
                .WithMany()
                .HasForeignKey(e => e.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 