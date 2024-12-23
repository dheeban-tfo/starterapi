using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Configurations;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class SocietyConfiguration : BaseConfiguration<Society>
    {
        public override void Configure(EntityTypeBuilder<Society> builder)
        {
            base.Configure(builder);

            builder.ToTable("Societies");
            
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(s => s.ContactNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(s => s.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.HasMany(s => s.Blocks)
                .WithOne(b => b.Society)
                .HasForeignKey(b => b.SocietyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 