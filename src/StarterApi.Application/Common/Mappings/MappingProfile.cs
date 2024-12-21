using AutoMapper;
using StarterApi.Application.Modules.Tenants;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tenant, TenantDto>()
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<Tenant, TenantInternalDto>();
        CreateMap<TenantUser, UserDto>()
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<CreateUserDto, TenantUser>();
        CreateMap<CreateUserDto, User>();
    }
} 