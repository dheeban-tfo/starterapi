using AutoMapper;
using StarterApi.Application.Modules.Tenants;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Units.DTOs;
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

        // Society mappings
        CreateMap<Society, SocietyDto>();
        CreateMap<CreateSocietyDto, Society>();
        CreateMap<UpdateSocietyDto, Society>();

        // Block mappings
        CreateMap<Block, BlockDto>()
            .ForMember(dest => dest.SocietyName, opt => opt.MapFrom(src => src.Society.Name));
        CreateMap<CreateBlockDto, Block>();
        CreateMap<UpdateBlockDto, Block>();

        // Floor mappings
        CreateMap<Floor, FloorDto>()
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Block.Name))
            .ForMember(dest => dest.BlockCode, opt => opt.MapFrom(src => src.Block.Code));
        CreateMap<CreateFloorDto, Floor>();
        CreateMap<UpdateFloorDto, Floor>();

        // Unit mappings
        CreateMap<Unit, UnitDto>();
        CreateMap<CreateUnitDto, Unit>();
        CreateMap<UpdateUnitDto, Unit>();
    }
} 