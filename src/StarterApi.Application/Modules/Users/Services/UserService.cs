using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTenantRepository _userTenantRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IUserTenantRepository userTenantRepository,
            ITenantRepository tenantRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _userTenantRepository = userTenantRepository;
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException($"User with email {dto.Email} already exists");

            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = dto.PasswordHash,
                UserType = dto.UserType,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> AddUserToTenantAsync(Guid userId, Guid tenantId, Guid roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found");

            var tenant = await _tenantRepository.GetByIdAsync(tenantId);
            if (tenant == null)
                throw new NotFoundException($"Tenant with ID {tenantId} not found");

            if (await _userTenantRepository.ExistsAsync(userId, tenantId))
                throw new InvalidOperationException("User already has access to this tenant");

            var userTenant = new UserTenant(user, tenant, roleId);
            await _userTenantRepository.AddAsync(userTenant);
            await _userTenantRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        // Implement other interface methods...
    }
}