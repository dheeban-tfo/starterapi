using AutoMapper;
using StarterApi.Application.Modules.Tenants;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Domain.Entities;
using StarterApi.Application.Modules.Individuals.DTOs;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Residents.DTOs;

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
        CreateMap<Society, SocietyDto>()
            .ForMember(dest => dest.TotalBlocks, opt => opt.MapFrom(src => src.Blocks.Count(b => b.IsActive)));
        CreateMap<CreateSocietyDto, Society>();
        CreateMap<UpdateSocietyDto, Society>();

        // Block mappings
        CreateMap<Block, BlockDto>()
            .ForMember(dest => dest.SocietyName, opt => opt.MapFrom(src => src.Society.Name))
            .ForMember(dest => dest.TotalFloors, opt => opt.MapFrom(src => src.Floors.Count(f => f.IsActive)));
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
        CreateMap<Unit, UnitLookupDto>()
            .ForMember(dest => dest.FloorName, opt => opt.MapFrom(src => src.Floor.FloorName))
            .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Floor.Block.Name));

        // Individual mappings
        CreateMap<Individual, IndividualDto>();
        CreateMap<CreateIndividualDto, Individual>();
        CreateMap<UpdateIndividualDto, Individual>();
        CreateMap<Individual, IndividualLookupDto>();

        // Resident mappings
        CreateMap<Resident, ResidentDto>();
        CreateMap<CreateResidentDto, Resident>();
        CreateMap<UpdateResidentDto, Resident>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 