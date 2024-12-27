using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Common.Mappings
{
    public class DetailMappingProfile : Profile
    {
        public DetailMappingProfile()
        {
            // Base mappings for lookup properties
            CreateMap<Individual, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<Unit, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UnitNumber))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UnitNumber));

            CreateMap<Block, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Floor, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FloorName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FloorName));

            CreateMap<Society, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Owner, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Individual.FullName))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Individual.FullName));

            CreateMap<Role, LookupDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Description));

            // Detail DTO mappings
            CreateMap<Individual, IndividualDetailDto>();

            CreateMap<Unit, UnitDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UnitNumber))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UnitNumber))
                .ForMember(dest => dest.SelectedFloor, opt => opt.MapFrom(src => src.Floor))
                .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Floor.Block))
                .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Floor.Block.Society))
                .ForMember(dest => dest.SelectedCurrentOwner, opt => opt.MapFrom(src => src.CurrentOwner));

            CreateMap<Block, BlockDetailDto>()
                .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Society))
                .ForMember(dest => dest.FloorCount, opt => opt.MapFrom(src => src.Floors.Count))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Floors.Sum(f => f.Units.Count)));

            CreateMap<Floor, FloorDetailDto>()
                .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Block))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Units.Count));

            CreateMap<Resident, ResidentDetailDto>()
                .ForMember(dest => dest.SelectedIndividual, opt => opt.MapFrom(src => src.Individual))
                .ForMember(dest => dest.SelectedUnit, opt => opt.MapFrom(src => src.Unit));

            CreateMap<TenantUser, UserDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.SelectedRole, opt => opt.MapFrom(src => src.Role));

            CreateMap<Role, RoleDetailDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission.SystemName)));

            CreateMap<Society, SocietyDetailDto>()
                .ForMember(dest => dest.BlockCount, opt => opt.MapFrom(src => src.Blocks.Count))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Blocks.Sum(b => b.Floors.Sum(f => f.Units.Count))));

            CreateMap<Owner, OwnerDetailDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Individual.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Individual.PhoneNumber))
                .ForMember(dest => dest.SelectedIndividual, opt => opt.MapFrom(src => src.Individual));
        }
    }
}
