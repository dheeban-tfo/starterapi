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

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FacilityId)
                .IsRequired();

            builder.Property(x => x.DocumentId)
                .IsRequired();

            builder.Property(x => x.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationships
            builder.HasOne(x => x.Facility)
                .WithMany(f => f.Images)
                .HasForeignKey(x => x.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Document)
                .WithMany()
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 