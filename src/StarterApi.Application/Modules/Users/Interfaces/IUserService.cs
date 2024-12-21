using StarterApi.Application.Modules.Users.DTOs;

namespace StarterApi.Application.Modules.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeactivateUserAsync(Guid id);
        Task<bool> AddUserToTenantAsync(Guid userId, Guid tenantId, Guid roleId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
    }
}