using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Tenants;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Domain.Entities;
using StarterApi.Application.Modules.Individuals.DTOs;
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
        CreateMap<Society, SocietyDto>();
        CreateMap<CreateSocietyDto, Society>();
        CreateMap<UpdateSocietyDto, Society>();

        // Block mappings
        CreateMap<Block, BlockDto>()
            .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Society))
            .ForMember(dest => dest.FloorCount, opt => opt.MapFrom(src => src.Floors.Count(f => f.IsActive)))
            .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Floors.Sum(f => f.Units.Count(u => u.IsActive))));
        CreateMap<CreateBlockDto, Block>();
        CreateMap<UpdateBlockDto, Block>();

        // Floor mappings
        CreateMap<Floor, FloorDto>()
            .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Block))
            .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Units.Count(u => u.IsActive)));
        CreateMap<CreateFloorDto, Floor>();
        CreateMap<UpdateFloorDto, Floor>();

        // Unit mappings
        CreateMap<Unit, UnitDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UnitNumber))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UnitNumber))
            .ForMember(dest => dest.SelectedFloor, opt => opt.MapFrom(src => src.Floor))
            .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Floor.Block))
            .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Floor.Block.Society))
            .ForMember(dest => dest.SelectedCurrentOwner, opt => opt.MapFrom(src => src.CurrentOwner));
        CreateMap<CreateUnitDto, Unit>();
        CreateMap<UpdateUnitDto, Unit>();

        // Individual mappings
        CreateMap<Individual, IndividualDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<CreateIndividualDto, Individual>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<UpdateIndividualDto, Individual>();

        // Resident mappings
        CreateMap<Resident, ResidentDto>()
            .ForMember(dest => dest.SelectedIndividual, opt => opt.MapFrom(src => src.Individual))
            .ForMember(dest => dest.SelectedUnit, opt => opt.MapFrom(src => src.Unit));
        CreateMap<CreateResidentDto, Resident>();
        CreateMap<UpdateResidentDto, Resident>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 