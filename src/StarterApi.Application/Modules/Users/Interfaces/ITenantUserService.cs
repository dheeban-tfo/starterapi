using StarterApi.Application.Modules.Users.DTOs;

public interface ITenantUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid id);
} 