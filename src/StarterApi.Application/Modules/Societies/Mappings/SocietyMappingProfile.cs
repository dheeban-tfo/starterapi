using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Societies.Mappings
{
    public class SocietyMappingProfile : Profile
    {
        public SocietyMappingProfile()
        {
            CreateMap<Society, SocietyDto>();

            CreateMap<Society, SocietyListDto>()
                .ForMember(dest => dest.BlockCount, opt => opt.MapFrom(src => src.Blocks.Count(b => b.IsActive)))
                .ForMember(dest => dest.UnitCount, opt => opt.MapFrom(src => src.Blocks.Sum(b => b.Floors.Sum(f => f.Units.Count(u => u.IsActive)))));

            CreateMap<CreateSocietyDto, Society>();
            CreateMap<UpdateSocietyDto, Society>();
        }
    }
}
