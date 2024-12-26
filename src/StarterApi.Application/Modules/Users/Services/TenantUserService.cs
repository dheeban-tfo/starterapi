using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Users.DTOs;

using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Users.Services
{
    public class TenantUserService : ITenantUserService
    {
        private readonly ITenantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TenantUserService> _logger;
        private readonly ITenantProvider _tenantProvider;
        private readonly IUserTenantRepository _userTenantRepository;
        private readonly IUserRepository _userRepository;

        public TenantUserService(
            ITenantDbContext context,
            IMapper mapper,
            ILogger<TenantUserService> logger,
            ITenantProvider tenantProvider,
            IUserTenantRepository userTenantRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _tenantProvider = tenantProvider;
            _userTenantRepository = userTenantRepository;
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                // Get default role first
                var defaultRoleId = await GetDefaultRoleId();

                // 1. Create user in tenant database first
                var tenantUser = new TenantUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    MobileNumber = dto.MobileNumber,
                    RoleId = defaultRoleId,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(tenantUser);
                await _context.SaveChangesAsync();

                // 2. Create user in root database
                var rootUser = new User
                {
                    Id = tenantUser.Id,  // Use same ID
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    MobileNumber = dto.MobileNumber,
                    IsActive = true
                };

                await _userRepository.AddAsync(rootUser);
                await _userRepository.SaveChangesAsync();

                // Add a small delay to ensure database operations complete
                await Task.Delay(100);

                // 3. Create UserTenant mapping
                var userTenant = new UserTenant
                {
                    UserId = tenantUser.Id,
                    TenantId = _tenantProvider.GetCurrentTenantId().Value,
                    RoleId = defaultRoleId, 
                    CreatedAt = DateTime.UtcNow
                };

                await _userTenantRepository.AddAsync(userTenant);
                await _userTenantRepository.SaveChangesAsync();

                return _mapper.Map<UserDto>(tenantUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user in tenant {TenantId}", _tenantProvider.GetCurrentTenantId());
                throw;
            }
        }
        // public async Task<IEnumerable<UserDto>> GetUsersAsync()
        // {
        //     var users = await _context.Users.ToListAsync();
        //     return _mapper.Map<IEnumerable<UserDto>>(users);
        // }

        public async Task<IEnumerable<UserProfileDto>> GetUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    
                    IsActive = u.IsActive,
                    LastLoginAt = u.LastLoginAt,
                    RoleName = u.Role.Name,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return _mapper.Map<UserDto>(user);
        }

       private async Task<Guid> GetDefaultRoleId()
        {
            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "User");
            
            if (defaultRole == null)
                throw new NotFoundException("Default role not found");
                
            return defaultRole.Id;
        }

        public async Task<UserRoleDto> GetUserRoleAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            return new UserRoleDto
            {
                UserId = user.Id,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }

        public async Task<UserRoleDto> UpdateUserRoleAsync(Guid userId, UpdateUserRoleDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
                throw new NotFoundException($"Role with ID {dto.RoleId} not found");

            user.RoleId = dto.RoleId;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UserRoleDto
            {
                UserId = user.Id,
                RoleId = user.RoleId,
                RoleName = role.Name
            };
        }

        public async Task<List<UsersByRoleDto>> GetUsersByRoleAsync(Guid roleId)
        {
            var users = await _context.Users
                .Where(u => u.RoleId == roleId)
                .Select(u => new UsersByRoleDto
                {
                    UserId = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserProfileDto> InviteUserAsync(UserInvitationDto dto)
        {
            try
            {
                // Create user in tenant database
                var tenantUser = new TenantUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    MobileNumber = dto.MobileNumber,
                    RoleId = dto.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(tenantUser);
                await _context.SaveChangesAsync();

                try
                {
                    // Create user in root database
                    var rootUser = new User
                    {
                        Id = tenantUser.Id,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Email = dto.Email,
                        MobileNumber = dto.MobileNumber,
                        IsActive = true
                    };

                    await _userRepository.AddAsync(rootUser);
                    await _userRepository.SaveChangesAsync();

                    // Add a small delay to ensure database operations complete
                    await Task.Delay(100);

                    // Create UserTenant mapping
                    var userTenant = new UserTenant
                    {
                        UserId = tenantUser.Id,
                        TenantId = _tenantProvider.GetCurrentTenantId().Value,
                        RoleId = dto.RoleId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _userTenantRepository.AddAsync(userTenant);
                    await _userTenantRepository.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // If root user creation fails, rollback tenant user
                    _context.Users.Remove(tenantUser);
                    await _context.SaveChangesAsync();
                    throw;
                }

                // Send invitation SMS
                // This would integrate with your SMS service
                await SendInvitationSms(dto.MobileNumber);

                return await GetUserProfileAsync(tenantUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inviting user");
                throw;
            }
        }

        public async Task<bool> ToggleUserStatusAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update root user status
            var rootUser = await _userRepository.GetByIdAsync(userId);
            if (rootUser != null)
            {
                rootUser.IsActive = user.IsActive;
                await _userRepository.SaveChangesAsync();
            }

            return user.IsActive;
        }

        private async Task SendInvitationSms(string mobileNumber)
        {
            // Implementation would depend on your SMS service
            // For now, just log it
            _logger.LogInformation("Invitation SMS would be sent to {MobileNumber}", mobileNumber);
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            return new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
               
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                RoleName = user.Role.Name,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UserProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Update root user as well
            var rootUser = await _userRepository.GetByIdAsync(userId);
            if (rootUser != null)
            {
                rootUser.FirstName = dto.FirstName;
                rootUser.LastName = dto.LastName;
                rootUser.Email = dto.Email;
                await _userRepository.SaveChangesAsync();
            }

            return await GetUserProfileAsync(userId);
        }
    }
} 