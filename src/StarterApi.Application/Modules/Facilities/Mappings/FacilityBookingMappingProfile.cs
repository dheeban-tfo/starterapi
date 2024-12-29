using AutoMapper;
using StarterApi.Application.Common.Models;
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
                    opt => opt.MapFrom(src => new FacilityLookupDto 
                    { 
                        Id = src.Facility.Id,
                        Name = src.Facility.Name,
                        Type = src.Facility.Type.ToString(),
                        Location = src.Facility.Location,
                        Status = src.Facility.Status.ToString()
                    }))
                .ForMember(dest => dest.SelectedResident,
                    opt => opt.MapFrom(src => new ResidentLookupDto 
                    { 
                        Id = src.Resident.Id,
                        FullName = $"{src.Resident.Individual.FirstName} {src.Resident.Individual.LastName}",
                        UnitNumber = src.Resident.Unit.UnitNumber,
                        ResidentType = src.Resident.ResidentType,
                        Status = src.Resident.Status
                    }));

            // Add mappings for lookup DTOs
            CreateMap<Facility, FacilityLookupDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Resident, ResidentLookupDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                    $"{src.Individual.FirstName} {src.Individual.LastName}"))
                .ForMember(dest => dest.UnitNumber, opt => opt.MapFrom(src => src.Unit.UnitNumber));
        }
    }
} 