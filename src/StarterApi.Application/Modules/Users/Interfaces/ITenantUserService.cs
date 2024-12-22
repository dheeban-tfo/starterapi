using StarterApi.Application.Modules.Users.DTOs;

public interface ITenantUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserRoleDto> GetUserRoleAsync(Guid userId);
    Task<UserRoleDto> UpdateUserRoleAsync(Guid userId, UpdateUserRoleDto dto);
    Task<List<UsersByRoleDto>> GetUsersByRoleAsync(Guid roleId);
} 