using AutoMapper;
using StarterApi.Domain.Entities;
using StarterApi.Application.Modules.Visitors.DTOs;

namespace StarterApi.Application.Modules.Visitors.Mappings
{
    public class VisitorMappingProfile : Profile
    {
        public VisitorMappingProfile()
        {
            // List mapping
            CreateMap<Visitor, VisitorListDto>()
                .ForMember(dest => dest.ResidentName, 
                    opt => opt.MapFrom(src => 
                        src.Resident != null 
                            ? $"{src.Resident.FirstName} {src.Resident.LastName}"
                            : string.Empty));

            // Detail mapping
            CreateMap<Visitor, VisitorDto>()
                .ForMember(dest => dest.SelectedResident, 
                    opt => opt.MapFrom(src => src.Resident));

        }
    }
} 