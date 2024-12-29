using AutoMapper;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Mappings
{
    public class FacilityMappingProfile : Profile
    {
        public FacilityMappingProfile()
        {
            // List mapping
            CreateMap<Facility, FacilityListDto>()
                .ForMember(dest => dest.SocietyName, opt => opt.MapFrom(src => src.Society.Name));

            // Detail mapping
            CreateMap<Facility, FacilityDto>()
                .ForMember(dest => dest.SelectedSociety, opt => opt.MapFrom(src => src.Society));

            // Create mapping
            CreateMap<CreateFacilityDto, Facility>();

            // Update mapping
            CreateMap<UpdateFacilityDto, Facility>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Booking rule mappings
            CreateMap<FacilityBookingRule, FacilityBookingRuleDto>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString(@"hh\:mm")))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString(@"hh\:mm")));

            CreateMap<UpdateFacilityBookingRuleDto, FacilityBookingRule>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => TimeSpan.Parse(src.EndTime)));

            // Blackout date mappings
            CreateMap<FacilityBlackoutDate, FacilityBlackoutDateDto>();
            CreateMap<UpdateFacilityBlackoutDateDto, FacilityBlackoutDate>();

            // Facility image mappings
            CreateMap<FacilityImage, FacilityImageDto>();
        }
    }
} 