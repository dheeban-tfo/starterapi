using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class FacilityImageConfiguration : IEntityTypeConfiguration<FacilityImage>
    {
        public void Configure(EntityTypeBuilder<FacilityImage> builder)
        {
            builder.ToTable("FacilityImages");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.FileSize)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(e => e.IsPrimary)
                .HasDefaultValue(false);

            builder.HasOne(e => e.Facility)
                .WithMany(f => f.Images)
                .HasForeignKey(e => e.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add common properties from BaseEntity
            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
} 