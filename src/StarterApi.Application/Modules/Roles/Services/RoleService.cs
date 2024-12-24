using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Roles.Interfaces;
using StarterApi.Domain.Entities;


namespace StarterApi.Application.Modules.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly ITenantDbContext _context;
        private readonly IRootDbContext _rootDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;
        private readonly IUserTenantRepository _userTenantRepository;

        public RoleService(
            ITenantDbContext context,
            IRootDbContext rootDbContext,
            IMapper mapper,
            ILogger<RoleService> logger,
            IUserTenantRepository userTenantRepository)
        {
            _context = context;
            _rootDbContext = rootDbContext;
            _mapper = mapper;
            _logger = logger;
            _userTenantRepository = userTenantRepository;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            if (await _context.Roles.AnyAsync(r => r.Name == dto.Name))
                throw new ValidationException($"Role with name {dto.Name} already exists");

            // Create role
            var role = new TenantRole
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            if (dto.Permissions?.Any() == true)
            {
                // Get or create permissions
                var permissions = new List<TenantPermission>();
                foreach (var permissionName in dto.Permissions)
                {
                    var permission = await _context.Permissions
                        .FirstOrDefaultAsync(p => p.SystemName == permissionName)
                        ?? new TenantPermission
                        {
                            Name = permissionName,
                            SystemName = permissionName,
                            Description = $"Permission to {permissionName.ToLower()}",
                            Group = "Default",
                            IsEnabled = true,
                            IsSystem = false,
                            CreatedAt = DateTime.UtcNow
                        };

                    if (permission.Id == Guid.Empty)
                    {
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();
                    }

                    permissions.Add(permission);
                }

                // Create role permissions
                var rolePermissions = permissions.Select(p => new TenantRolePermission
                {
                    RoleId = role.Id,
                    PermissionId = p.Id,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.RolePermissions.AddRangeAsync(rolePermissions);
                await _context.SaveChangesAsync();
            }

            return await GetRoleByIdAsync(role.Id);
        }

        public async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            if (role.Name != dto.Name && await _context.Roles.AnyAsync(r => r.Name == dto.Name))
                throw new ValidationException($"Role with name {dto.Name} already exists");

            // Update basic properties
            role.Name = dto.Name;
            role.Description = dto.Description;
            role.UpdatedAt = DateTime.UtcNow;

            // Remove existing role permissions
            _context.RolePermissions.RemoveRange(role.RolePermissions);
            await _context.SaveChangesAsync();

            // Add new role permissions
            if (dto.Permissions?.Any() == true)
            {
                foreach (var permissionName in dto.Permissions)
                {
                    var permission = await _context.Permissions
                        .FirstOrDefaultAsync(p => p.SystemName == permissionName)
                        ?? new TenantPermission
                        {
                            Name = permissionName,
                            SystemName = permissionName,
                            Description = $"Permission to {permissionName.ToLower()}",
                            Group = "Default",
                            IsEnabled = true,
                            IsSystem = false,
                            CreatedAt = DateTime.UtcNow
                        };

                    if (permission.Id == Guid.Empty)
                    {
                        await _context.Permissions.AddAsync(permission);
                        await _context.SaveChangesAsync();
                    }

                    var rolePermission = new TenantRolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }
            }

            await _context.SaveChangesAsync();

            return await GetRoleByIdAsync(id);
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Name = p.Name,
                    SystemName = p.SystemName,
                    Description = p.Description,
                    Group = p.Group,
                    IsEnabled = p.IsEnabled,
                    IsSystem = p.IsSystem
                })
                .Distinct()
                .ToListAsync();

            return permissions;
        }

        public async Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();

            return _mapper.Map<List<PermissionDto>>(permissions);
        }

        public async Task UpdateRolePermissionsAsync(Guid roleId, RolePermissionUpdateDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new NotFoundException($"Role with ID {roleId} not found");

            // Clear existing permissions
            if (role.RolePermissions != null)
            {
                _context.RolePermissions.RemoveRange(role.RolePermissions);
            }

            // Add new permissions
            foreach (var permissionName in dto.Permissions)
            {
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.SystemName == permissionName)
                    ?? new TenantPermission
                    {
                        Name = permissionName,
                        SystemName = permissionName,
                        Description = $"Permission to {permissionName.ToLower()}",
                        Group = "Default",
                        IsEnabled = true,
                        IsSystem = false,
                        CreatedAt = DateTime.UtcNow
                    };

                if (permission.Id == Guid.Empty)
                {
                    await _context.Permissions.AddAsync(permission);
                    await _context.SaveChangesAsync();
                }

                var rolePermission = new TenantRolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.RolePermissions.AddAsync(rolePermission);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<PermissionDto>> GetUserPermissionsAsync(Guid userId)
        {
            _logger.LogInformation("Getting permissions for user: {UserId}", userId);

            // Check if root admin
            var rootUser = await _rootDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.UserType == UserType.RootAdmin);

            if (rootUser != null)
            {
                _logger.LogInformation("User {UserId} is root admin, returning all permissions", userId);
                // Get all permissions from root database
                var allPermissions = await _rootDbContext.Permissions
                    .AsNoTracking()
                    .Select(p => new PermissionDto
                    {
                        Name = p.SystemName,
                        SystemName = p.SystemName,
                        Description = p.Description,
                        Group = p.Group,
                        IsEnabled = true,
                        IsSystem = true
                    })
                    .ToListAsync();

                return allPermissions;
            }

            // Regular tenant user flow
            return await GetTenantUserPermissions(userId);
        }

        private async Task<List<PermissionDto>> GetTenantUserPermissions(Guid userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found in tenant database", userId);
                return new List<PermissionDto>();
            }

            if (user.Role == null)
            {
                _logger.LogWarning("User {UserId} has no assigned role", userId);
                return new List<PermissionDto>();
            }

            var permissions = user.Role.RolePermissions?.Select(rp => rp.Permission) ?? new List<TenantPermission>();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }

       
    }
}