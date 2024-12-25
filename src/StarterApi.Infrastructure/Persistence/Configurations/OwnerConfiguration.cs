using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class OwnerConfiguration : BaseConfiguration<Owner>
    {
        public override void Configure(EntityTypeBuilder<Owner> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Owners");
            
            builder.HasKey(o => o.Id);
            
            builder.Property(o => o.IndividualId)
                .IsRequired();
                
            builder.Property(o => o.OwnershipType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(o => o.OwnershipStartDate)
                .IsRequired();
                
            builder.HasOne(o => o.Individual)
                .WithMany(i => i.Owners)
                .HasForeignKey(o => o.IndividualId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 