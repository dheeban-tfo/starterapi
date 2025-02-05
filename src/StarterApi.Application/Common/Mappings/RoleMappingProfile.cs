using AutoMapper;

using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<TenantRole, RoleDto>()
                .ForMember(d => d.Permissions, opt => opt
                    .MapFrom(s => s.RolePermissions.Select(rp => rp.Permission.SystemName)));

            CreateMap<TenantPermission, PermissionDto>();
            
            CreateMap<CreateRoleDto, TenantRole>();
            CreateMap<UpdateRoleDto, TenantRole>();
        }
    }
} 