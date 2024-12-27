using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Blocks.Mappings
{
    public class BlockMappingProfile : Profile
    {
        public BlockMappingProfile()
        {
            CreateMap<Block, BlockDto>()
                .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Society))
                .ForMember(dest => dest.FloorCount, opt => opt.MapFrom(src => src.Floors.Count(f => f.IsActive)))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Floors.Sum(f => f.Units.Count(u => u.IsActive))));

            CreateMap<Block, BlockListDto>()
                .ForMember(dest => dest.SocietyName, opt => opt.MapFrom(src => src.Society.Name))
                .ForMember(dest => dest.FloorCount, opt => opt.MapFrom(src => src.Floors.Count(f => f.IsActive)))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Floors.Sum(f => f.Units.Count(u => u.IsActive))));

            CreateMap<CreateBlockDto, Block>();
            CreateMap<UpdateBlockDto, Block>();
        }
    }
}
