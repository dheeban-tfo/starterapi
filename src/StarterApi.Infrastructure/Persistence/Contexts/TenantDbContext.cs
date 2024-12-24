using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Configurations;

namespace StarterApi.Infrastructure.Persistence.Contexts
{
    public class TenantDbContext : DbContext, ITenantDbContext
    {
        private readonly string _connectionString;

        public DbSet<TenantUser> Users { get; set; }
        public DbSet<TenantRole> Roles { get; set; }
        public DbSet<TenantPermission> Permissions { get; set; }
        public DbSet<TenantRolePermission> RolePermissions { get; set; }
        
        // Society Management
        public DbSet<Society> Societies { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Unit> Units { get; set; }
        
        // People
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Resident> Residents { get; set; }
        
        // Contracts and Bookings
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<FacilityBooking> FacilityBookings { get; set; }
        
        // Facilities and Parking
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<ParkingAllocation> ParkingAllocations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        
        // Communication and Management
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        // Auditing
        public DbSet<AuditHistory> AuditHistory { get; set; }

        public TenantDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TenantUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.MobileNumber).IsRequired();
            });

            modelBuilder.Entity<TenantRole>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TenantPermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.SystemName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Group).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<TenantRolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Apply new configurations
            modelBuilder.ApplyConfiguration(new SocietyConfiguration());
            modelBuilder.ApplyConfiguration(new BlockConfiguration());
            modelBuilder.ApplyConfiguration(new FloorConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
            modelBuilder.ApplyConfiguration(new ResidentConfiguration());
            modelBuilder.ApplyConfiguration(new RentalContractConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityBookingConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingSlotConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingAllocationConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VisitorConfiguration());
            modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
            modelBuilder.ApplyConfiguration(new ComplaintConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new AuditHistoryConfiguration());
        }
    }
}