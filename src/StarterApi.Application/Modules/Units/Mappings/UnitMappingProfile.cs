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

            CreateMap<CreateUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();
        }
    }
}
