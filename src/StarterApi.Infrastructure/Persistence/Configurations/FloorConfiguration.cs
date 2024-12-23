using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Configurations
{
    public class FloorConfiguration : BaseConfiguration<Floor>
    {
        public override void Configure(EntityTypeBuilder<Floor> builder)
        {
            base.Configure(builder);

            builder.ToTable("Floors");

            builder.HasOne(f => f.Block)
                .WithMany(b => b.Floors)
                .HasForeignKey(f => f.BlockId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 