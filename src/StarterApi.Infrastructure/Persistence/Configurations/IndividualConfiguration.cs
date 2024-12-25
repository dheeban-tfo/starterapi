using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class IndividualConfiguration : BaseConfiguration<Individual>
    {
        public override void Configure(EntityTypeBuilder<Individual> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Individuals");
            
            builder.Property(i => i.FirstName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(i => i.LastName)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(i => i.FullName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(i => i.Gender)
                .HasMaxLength(20);
                
            builder.Property(i => i.Email)
                .HasMaxLength(100);
                
            builder.Property(i => i.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
                
            builder.Property(i => i.AlternatePhoneNumber)
                .HasMaxLength(20);
                
            builder.Property(i => i.IdProofType)
                .HasMaxLength(50);
                
            builder.Property(i => i.IdProofNumber)
                .HasMaxLength(100);
                
            builder.Property(i => i.AddressLine1)
                .HasMaxLength(100);
                
            builder.Property(i => i.AddressLine2)
                .HasMaxLength(100);
                
            builder.Property(i => i.City)
                .HasMaxLength(50);
                
            builder.Property(i => i.State)
                .HasMaxLength(50);
                
            builder.Property(i => i.Country)
                .HasMaxLength(50);
                
            builder.Property(i => i.Pincode)
                .HasMaxLength(10);
                
            builder.Property(i => i.EmergencyContactName)
                .HasMaxLength(100);
                
            builder.Property(i => i.EmergencyContactNumber)
                .HasMaxLength(20);

            // Indexes
            builder.HasIndex(i => i.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
            builder.HasIndex(i => i.PhoneNumber).IsUnique();
            builder.HasIndex(i => new { i.IdProofType, i.IdProofNumber }).IsUnique().HasFilter("[IdProofType] IS NOT NULL AND [IdProofNumber] IS NOT NULL");
        }
    }
}
