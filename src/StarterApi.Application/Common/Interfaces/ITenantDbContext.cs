using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Interfaces
{
    public interface ITenantDbContext
    {
        DbSet<TenantUser> Users { get; set; }
        DbSet<TenantRole> Roles { get; set; }
        DbSet<TenantPermission> Permissions { get; set; }
        DbSet<TenantRolePermission> RolePermissions { get; set; }
        
        // Society Management
        DbSet<Society> Societies { get; set; }
        DbSet<Block> Blocks { get; set; }
        DbSet<Floor> Floors { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<Individual> Individuals { get; set; }
        
        // People
        DbSet<Owner> Owners { get; set; }
        DbSet<Resident> Residents { get; set; }
        
        // Contracts and Bookings
        DbSet<RentalContract> RentalContracts { get; set; }
        DbSet<FacilityBooking> FacilityBookings { get; set; }
        
        // Facilities and Parking
        DbSet<Facility> Facilities { get; set; }
        DbSet<ParkingSlot> ParkingSlots { get; set; }
        DbSet<ParkingAllocation> ParkingAllocations { get; set; }
        DbSet<Vehicle> Vehicles { get; set; }
        
        // Communication and Management
        DbSet<Visitor> Visitors { get; set; }
        DbSet<Announcement> Announcements { get; set; }
        DbSet<Complaint> Complaints { get; set; }
        DbSet<Notification> Notifications { get; set; }
        
        // Document Management
        DbSet<Document> Documents { get; set; }
        DbSet<DocumentVersion> DocumentVersions { get; set; }
        DbSet<DocumentCategory> DocumentCategories { get; set; }
        DbSet<DocumentAccess> DocumentAccesses { get; set; }
        
        // Auditing
        DbSet<AuditHistory> AuditHistory { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 