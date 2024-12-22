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
                .Include(r => r.Permissions)
                .ToListAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            if (await _context.Roles.AnyAsync(r => r.Name == dto.Name))
                throw new ValidationException($"Role with name {dto.Name} already exists");

            var role = new TenantRole
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                Permissions = dto.Permissions?.Select(p => new TenantPermission
                {
                    Name = p,
                    SystemName = p,
                    Description = $"Permission to {p.ToLower()}",
                    Group = "Default",
                    IsEnabled = true,
                    IsSystem = false,
                    CreatedAt = DateTime.UtcNow
                }).ToList() ?? new List<TenantPermission>()
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            if (role.Name != dto.Name && await _context.Roles.AnyAsync(r => r.Name == dto.Name))
                throw new ValidationException($"Role with name {dto.Name} already exists");

            // Update basic properties
            role.Name = dto.Name;
            role.Description = dto.Description;
            role.UpdatedAt = DateTime.UtcNow;

            // Update permissions
            if (role.Permissions != null)
            {
                // Get existing permissions
                var existingPermissions = await _context.Permissions
                    .Where(p => p.TenantRoleId == id)
                    .ToListAsync();

                // Remove existing permissions
                _context.Permissions.RemoveRange(existingPermissions);
                await _context.SaveChangesAsync(); // Save the removal first
            }

            // Add new permissions
            var newPermissions = dto.Permissions?.Select(p => new TenantPermission
            {
                Name = p,
                SystemName = p,
                Description = $"Permission to {p.ToLower()}",
                Group = "Default",
                IsEnabled = true,
                IsSystem = false,
                TenantRoleId = id,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? new List<TenantPermission>();

            // Add new permissions
            await _context.Permissions.AddRangeAsync(newPermissions);
            
            // Save all changes
            await _context.SaveChangesAsync();

            // Reload the role with updated permissions
            role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);

            return _mapper.Map<RoleDto>(role);
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
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new NotFoundException($"Role with ID {roleId} not found");

            return _mapper.Map<List<PermissionDto>>(role.Permissions);
        }

        public async Task UpdateRolePermissionsAsync(Guid roleId, RolePermissionUpdateDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new NotFoundException($"Role with ID {roleId} not found");

            // Clear existing permissions
            if (role.Permissions != null)
            {
                var existingPermissions = await _context.Permissions
                    .Where(p => p.TenantRoleId == roleId)
                    .ToListAsync();
                    
                _context.Permissions.RemoveRange(existingPermissions);
            }

            // Add new permissions
            var newPermissions = dto.Permissions.Select(p => new TenantPermission
            {
                Name = p,
                SystemName = p,
                Description = $"Permission to {p.ToLower()}",
                Group = "Default",
                IsEnabled = true,
                IsSystem = false,
                TenantRoleId = roleId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.Permissions.AddRangeAsync(newPermissions);
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
                    .ThenInclude(r => r.Permissions)
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

            var permissions = user.Role.Permissions ?? new List<TenantPermission>();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}