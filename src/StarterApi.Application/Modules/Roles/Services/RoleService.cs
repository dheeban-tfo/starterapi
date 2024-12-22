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
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            ITenantDbContext context,
            IMapper mapper,
            ILogger<RoleService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
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

            role.Name = dto.Name;
            role.Description = dto.Description;
            role.UpdatedAt = DateTime.UtcNow;

            // Update permissions
            _context.Permissions.RemoveRange(role.Permissions);
            role.Permissions = dto.Permissions?.Select(p => new TenantPermission
            {
                Name = p,
                SystemName = p,
                Group = "Default",
                IsEnabled = true,
                IsSystem = false,
                CreatedAt = DateTime.UtcNow
            }).ToList() ?? new List<TenantPermission>();

            await _context.SaveChangesAsync();

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
    }
}