using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Floors.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Floors.Mappings
{
    public class FloorMappingProfile : Profile
    {
        public FloorMappingProfile()
        {
            CreateMap<Floor, FloorDto>()
                .ForMember(dest => dest.SelectedBlock, opt => opt.MapFrom(src => src.Block))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Units.Count(u => u.IsActive)));

            CreateMap<Floor, FloorListDto>()
                .ForMember(dest => dest.BlockName, opt => opt.MapFrom(src => src.Block.Name))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Units.Count(u => u.IsActive)));

            CreateMap<CreateFloorDto, Floor>();
            CreateMap<UpdateFloorDto, Floor>();
        }
    }
}
