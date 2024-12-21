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

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber));

        CreateMap<TenantUser, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber));
    }
} 