namespace StarterApi.Application.Modules.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto dto);
        Task DeleteRoleAsync(Guid id);

        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId);
        Task UpdateRolePermissionsAsync(Guid roleId, RolePermissionUpdateDto dto);
        Task<List<PermissionDto>> GetUserPermissionsAsync(Guid userId,Guid? tenantId = null);
    }
} 