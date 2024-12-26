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
            
            builder.Property(r => r.ResidentType)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(r => r.RelationToOwner)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            // Relationships
            builder.HasOne(r => r.Unit)
                .WithMany(u => u.Residents)
                .HasForeignKey(r => r.UnitId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Individual)
                .WithMany()
                .HasForeignKey(r => r.IndividualId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<TenantUser>("User")
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.Vehicles)
                .WithOne(v => v.Resident)
                .HasForeignKey(v => v.ResidentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.Documents)
                .WithOne()
                .HasForeignKey("ResidentId")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
} 