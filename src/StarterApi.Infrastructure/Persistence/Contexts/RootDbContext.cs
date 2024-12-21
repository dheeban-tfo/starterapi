using Microsoft.EntityFrameworkCore;

using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Contexts
{
    public class RootDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTenant> UserTenants { get; set; }
        public DbSet<OtpRequest> OtpRequests { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public RootDbContext(DbContextOptions<RootDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTenant>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TenantId });

                entity.HasOne(ut => ut.User)
                    .WithMany(u => u.UserTenants)
                    .HasForeignKey(ut => ut.UserId);

                entity.HasOne(ut => ut.Tenant)
                    .WithMany(t => t.UserTenants)
                    .HasForeignKey(ut => ut.TenantId);
            });

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId);
        }
    }
} 