using StarterApi.Application.Modules.Users.DTOs;

public interface ITenantUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    // Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserRoleDto> GetUserRoleAsync(Guid userId);
    Task<UserRoleDto> UpdateUserRoleAsync(Guid userId, UpdateUserRoleDto dto);
    Task<List<UsersByRoleDto>> GetUsersByRoleAsync(Guid roleId);
     Task<UserProfileDto> InviteUserAsync(UserInvitationDto dto);
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UserProfileDto dto);
    Task<bool> ToggleUserStatusAsync(Guid userId);
    Task<IEnumerable<UserProfileDto>> GetUsersAsync();
} 