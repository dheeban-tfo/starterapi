using AutoMapper;

using StarterApi.Domain.Entities;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDto>();
        CreateMap<Permission, PermissionDto>();
    }
} 