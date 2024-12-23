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
            
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(o => o.ContactNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(o => o.IDProofType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(o => o.IDProofNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(o => o.Units)
                .WithOne(u => u.CurrentOwner)
                .HasForeignKey(u => u.CurrentOwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 