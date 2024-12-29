using AutoMapper;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Mappings
{
    public class FacilityBookingMappingProfile : Profile
    {
        public FacilityBookingMappingProfile()
        {
            // List mapping
            CreateMap<FacilityBooking, FacilityBookingListDto>()
                .ForMember(dest => dest.FacilityName,
                    opt => opt.MapFrom(src => src.Facility.Name))
                .ForMember(dest => dest.ResidentName,
                    opt => opt.MapFrom(src => 
                        $"{src.Resident.Individual.FirstName} {src.Resident.Individual.LastName}"));

            // Detail mapping
            CreateMap<FacilityBooking, FacilityBookingDto>()
                .ForMember(dest => dest.SelectedFacility,
                    opt => opt.MapFrom(src => src.Facility))
                .ForMember(dest => dest.SelectedResident,
                    opt => opt.MapFrom(src => src.Resident));
        }
    }
} 