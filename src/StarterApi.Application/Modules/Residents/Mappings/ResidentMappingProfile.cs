using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Residents.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Residents.Mappings
{
    public class ResidentMappingProfile : Profile
    {
        public ResidentMappingProfile()
        {
            CreateMap<Resident, ResidentDto>()
                .ForMember(dest => dest.SelectedIndividual, opt => opt.MapFrom(src => src.Individual))
                .ForMember(dest => dest.SelectedUnit, opt => opt.MapFrom(src => src.Unit));

            CreateMap<Resident, ResidentListDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Individual.FirstName} {src.Individual.LastName}"))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.Unit.UnitNumber));

            CreateMap<CreateResidentDto, Resident>();
            CreateMap<UpdateResidentDto, Resident>();
        }
    }
}
