using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class ComplaintConfiguration : BaseConfiguration<Complaint>
    {
        public override void Configure(EntityTypeBuilder<Complaint> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Complaints");
            
            builder.Property(c => c.Category)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(1000);
                
            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(c => c.ResolutionDetails)
                .HasMaxLength(1000);

            builder.HasOne(c => c.Resident)
                .WithMany()
                .HasForeignKey(c => c.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.AssignedTo)
                .WithMany()
                .HasForeignKey(c => c.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 