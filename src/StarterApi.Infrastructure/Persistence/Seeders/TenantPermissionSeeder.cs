using Microsoft.EntityFrameworkCore;
using StarterApi.Domain.Constants;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace StarterApi.Infrastructure.Persistence.Seeders
{
    public class TenantPermissionSeeder
    {
        private readonly TenantDbContext _context;
        private readonly ILogger<TenantPermissionSeeder> _logger;

        public TenantPermissionSeeder(
            TenantDbContext context,
            ILogger<TenantPermissionSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting permission seeding/update");
                var permissions = new List<TenantPermission>();
                var existingPermissions = await _context.Permissions.ToListAsync();
                
                // Users permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Users", new[]
                {
                    (Permissions.Users.View, "View Users", "Can view users in the system"),
                    (Permissions.Users.Create, "Create Users", "Can create new users"),
                    (Permissions.Users.Edit, "Edit Users", "Can edit existing users"),
                    (Permissions.Users.Delete, "Delete Users", "Can delete users"),
                    (Permissions.Users.ManageRoles, "Manage User Roles", "Can manage user roles"),
                    (Permissions.Users.InviteUser, "Invite Users", "Can invite new users to the system")
                });

                // Roles permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Roles", new[]
                {
                    (Permissions.Roles.View, "View Roles", "Can view roles in the system"),
                    (Permissions.Roles.Create, "Create Roles", "Can create new roles"),
                    (Permissions.Roles.Edit, "Edit Roles", "Can edit existing roles"),
                    (Permissions.Roles.Delete, "Delete Roles", "Can delete roles"),
                    (Permissions.Roles.ManagePermissions, "Manage Role Permissions", "Can manage role permissions")
                });

                // Societies permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Societies", new[]
                {
                    (Permissions.Societies.View, "View Societies", "Can view societies in the system"),
                    (Permissions.Societies.Create, "Create Societies", "Can create new societies"),
                    (Permissions.Societies.Edit, "Edit Societies", "Can edit existing societies"),
                    (Permissions.Societies.Delete, "Delete Societies", "Can delete societies"),
                    (Permissions.Societies.ManageBlocks, "Manage Society Blocks", "Can manage society blocks")
                });

                // Blocks permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Blocks", new[]
                {
                    (Permissions.Blocks.View, "View Blocks", "Can view blocks in the system"),
                    (Permissions.Blocks.Create, "Create Blocks", "Can create new blocks"),
                    (Permissions.Blocks.Edit, "Edit Blocks", "Can edit existing blocks"),
                    (Permissions.Blocks.Delete, "Delete Blocks", "Can delete blocks"),
                    (Permissions.Blocks.ManageFloors, "Manage Block Floors", "Can manage block floors")
                });

                // Floors permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Floors", new[]
                {
                    (Permissions.Floors.View, "View Floors", "Can view floors in the system"),
                    (Permissions.Floors.Create, "Create Floors", "Can create new floors"),
                    (Permissions.Floors.Edit, "Edit Floors", "Can edit existing floors"),
                    (Permissions.Floors.Delete, "Delete Floors", "Can delete floors"),
                    (Permissions.Floors.ManageUnits, "Manage Floor Units", "Can manage floor units")
                });

                // Units permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Units", new[]
                {
                    (Permissions.Units.View, "View Units", "Can view units in the system"),
                    (Permissions.Units.Create, "Create Units", "Can create new units"),
                    (Permissions.Units.Edit, "Edit Units", "Can edit existing units"),
                    (Permissions.Units.Delete, "Delete Units", "Can delete units")
                });

                // Individuals permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Individuals", new[]
                {
                    (Permissions.Individuals.View, "View Individuals", "Can view individuals in the system"),
                    (Permissions.Individuals.Create, "Create Individuals", "Can create new individuals"),
                    (Permissions.Individuals.Edit, "Edit Individuals", "Can edit existing individuals"),
                    (Permissions.Individuals.Delete, "Delete Individuals", "Can delete individuals"),
                    (Permissions.Individuals.Verify, "Verify Individuals", "Can verify individuals")
                });

                // Residents permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Residents", new[]
                {
                    (Permissions.Residents.View, "View Residents", "Can view residents in the system"),
                    (Permissions.Residents.Create, "Create Residents", "Can create new residents"),
                    (Permissions.Residents.Edit, "Edit Residents", "Can edit existing residents"),
                    (Permissions.Residents.Delete, "Delete Residents", "Can delete residents"),
                    (Permissions.Residents.ManageFamily, "Manage Family", "Can manage resident family members"),
                    (Permissions.Residents.Verify, "Verify Residents", "Can verify residents")
                });

                // Documents permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Documents", new[]
                {
                    (Permissions.Documents.View, "View Documents", "Can view documents in the system"),
                    (Permissions.Documents.Create, "Create Documents", "Can create new documents"),
                    (Permissions.Documents.Edit, "Edit Documents", "Can edit existing documents"),
                    (Permissions.Documents.Delete, "Delete Documents", "Can delete documents"),
                    (Permissions.Documents.ManageCategories, "Manage Document Categories", "Can manage document categories"),
                    (Permissions.Documents.ManageAccess, "Manage Document Access", "Can manage document access"),
                    (Permissions.Documents.ManageVersions, "Manage Document Versions", "Can manage document versions")
                });

                // Facilities permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Facilities", new[]
                {
                    (Permissions.Facilities.View, "View Facilities", "Can view facilities in the system"),
                    (Permissions.Facilities.Create, "Create Facilities", "Can create new facilities"),
                    (Permissions.Facilities.Edit, "Edit Facilities", "Can edit existing facilities"),
                    (Permissions.Facilities.Delete, "Delete Facilities", "Can delete facilities"),
                    (Permissions.Facilities.ManageBookings, "Manage Bookings", "Can manage facility bookings"),
                    (Permissions.Facilities.ManageMaintenance, "Manage Maintenance", "Can manage facility maintenance")
                });

                // Facility Bookings permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "FacilityBookings", new[]
                {
                    (Permissions.FacilityBookings.View, "View Facility Bookings", "Can view facility bookings in the system"),
                    (Permissions.FacilityBookings.Create, "Create Facility Bookings", "Can create new facility bookings"),
                    (Permissions.FacilityBookings.Edit, "Edit Facility Bookings", "Can edit existing facility bookings"),
                    (Permissions.FacilityBookings.Cancel, "Cancel Facility Bookings", "Can cancel facility bookings"),
                    (Permissions.FacilityBookings.ManageOthers, "Manage Others' Bookings", "Can manage facility bookings made by others")
                });

                // Owners permissions
                await AddOrUpdatePermissionGroup(permissions, existingPermissions, "Owners", new[]
                {
                    (Permissions.Owners.View, "View Owners", "Can view owners in the system"),
                    (Permissions.Owners.Create, "Create Owners", "Can create new owners"),
                    (Permissions.Owners.Edit, "Edit Owners", "Can edit existing owners"),
                    (Permissions.Owners.Delete, "Delete Owners", "Can delete owners"),
                    (Permissions.Owners.ViewHistory, "View Owner History", "Can view owner history"),
                    (Permissions.Owners.ManageDocuments, "Manage Owner Documents", "Can manage owner documents"),
                    (Permissions.Owners.InitiateTransfer, "Initiate Ownership Transfer", "Can initiate ownership transfer"),
                    (Permissions.Owners.ApproveTransfer, "Approve Ownership Transfer", "Can approve ownership transfer")
                });

                if (permissions.Any())
                {
                    await _context.Permissions.AddRangeAsync(permissions);
                    await _context.SaveChangesAsync();
                    
                    // Assign new permissions to Root Admin role
                    var rootAdminRole = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Name == "Root Admin");
                    
                    if (rootAdminRole != null)
                    {
                        var existingRolePermissions = await _context.RolePermissions
                            .Where(rp => rp.RoleId == rootAdminRole.Id)
                            .Select(rp => rp.PermissionId)
                            .ToListAsync();

                        var newRolePermissions = permissions
                            .Where(p => !existingRolePermissions.Contains(p.Id))
                            .Select(p => new TenantRolePermission
                            {
                                RoleId = rootAdminRole.Id,
                                PermissionId = p.Id,
                                CreatedBy = rootAdminRole.CreatedBy,
                                CreatedAt = DateTime.UtcNow
                            });

                        await _context.RolePermissions.AddRangeAsync(newRolePermissions);
                        await _context.SaveChangesAsync();
                    }
                    
                    _logger.LogInformation("Permission seeding/update completed. Added {Count} new permissions", 
                        permissions.Count);
                }
                else
                {
                    _logger.LogInformation("No new permissions to add");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding permissions");
                throw;
            }
        }

        private async Task AddOrUpdatePermissionGroup(
            List<TenantPermission> newPermissions,
            List<TenantPermission> existingPermissions,
            string group,
            IEnumerable<(string SystemName, string Name, string Description)> groupPermissions)
        {
            foreach (var (systemName, name, description) in groupPermissions)
            {
                if (!existingPermissions.Any(p => p.SystemName == systemName))
                {
                    newPermissions.Add(new TenantPermission
                    {
                        Id = Guid.NewGuid(),
                        SystemName = systemName,
                        Name = name,
                        Description = description,
                        Group = group,
                        IsEnabled = true,
                        IsSystem = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
} 