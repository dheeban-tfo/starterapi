using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Units.Mappings
{
    public class UnitMappingProfile : Profile
    {
        public UnitMappingProfile()
        {
            CreateMap<Unit, UnitDto>()
                .ForMember(dest => dest.SelectedFloor, opt => opt.MapFrom(src => src.Floor))
                .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Floor.Block))
                .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Floor.Block.Society))
                .ForMember(dest => dest.SelectedCurrentOwner, opt => opt.MapFrom(src => src.CurrentOwner));

            CreateMap<Unit, UnitListDto>()
                .ForMember(dest => dest.FloorName, opt => opt.MapFrom(src => src.Floor.FloorName))
                .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Floor.Block.Name))
                .ForMember(dest => dest.CurrentOwnerName, opt => opt.MapFrom(src => $"{src.CurrentOwner.Individual.FirstName} {src.CurrentOwner.Individual.LastName}"));

            CreateMap<CreateUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();
        }
    }
}
