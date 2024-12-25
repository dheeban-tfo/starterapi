using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class ResidentConfiguration : BaseConfiguration<Resident>
    {
        public override void Configure(EntityTypeBuilder<Resident> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Residents");
            
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(r => r.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(r => r.ContactNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(r => r.Gender)
                .IsRequired()
                .HasMaxLength(10);
                
            builder.Property(r => r.RelationToOwner)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.IDProofType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.IDProofNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(r => r.Vehicles)
                .WithOne(v => v.Resident)
                .HasForeignKey(v => v.ResidentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Unit)
                .WithMany(u => u.Residents)
                .HasForeignKey(r => r.UnitId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 