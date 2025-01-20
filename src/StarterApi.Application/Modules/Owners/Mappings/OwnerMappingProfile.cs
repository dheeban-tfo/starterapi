using AutoMapper;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Owners.Mappings
{
    public class OwnerMappingProfile : Profile
    {
        public OwnerMappingProfile()
        {
            // Owner mappings
            CreateMap<Owner, OwnerListDto>()
                .ForMember(dest => dest.OwnerName, 
                    opt => opt.MapFrom(src => $"{src.Individual.FirstName} {src.Individual.LastName}"))
                .ForMember(dest => dest.UnitCount, 
                    opt => opt.MapFrom(src => src.Units.Count))
                .ForMember(dest => dest.ContactNumber,
                    opt => opt.MapFrom(src => src.Individual.PhoneNumber))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Individual.Email));

            CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.SelectedIndividual, 
                    opt => opt.MapFrom(src => src.Individual))
                .ForMember(dest => dest.Units,
                    opt => opt.MapFrom(src => src.Units))
                .ForMember(dest => dest.OwnershipHistory,
                    opt => opt.MapFrom(src => src.OwnershipHistory));
                

            // OwnershipHistory mappings
            CreateMap<OwnershipHistory, OwnershipHistoryListDto>()
                .ForMember(dest => dest.UnitNumber,
                    opt => opt.MapFrom(src => src.Unit.UnitNumber))
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src => $"{src.Owner.Individual.FirstName} {src.Owner.Individual.LastName}"));

            CreateMap<OwnershipHistory, OwnershipHistoryDto>()
                .ForMember(dest => dest.SelectedUnit,
                    opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.SelectedOwner,
                    opt => opt.MapFrom(src => src.Owner));

            // OwnershipTransfer mappings
            CreateMap<OwnershipTransferRequest, OwnershipTransferListDto>()
                .ForMember(dest => dest.UnitNumber,
                    opt => opt.MapFrom(src => src.Unit.UnitNumber))
                .ForMember(dest => dest.CurrentOwnerName,
                    opt => opt.MapFrom(src => $"{src.CurrentOwner.Individual.FirstName} {src.CurrentOwner.Individual.LastName}"))
                .ForMember(dest => dest.NewOwnerName,
                    opt => opt.MapFrom(src => $"{src.NewOwner.Individual.FirstName} {src.NewOwner.Individual.LastName}"));

            CreateMap<OwnershipTransferRequest, OwnershipTransferDto>()
                .ForMember(dest => dest.SelectedUnit,
                    opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.SelectedCurrentOwner,
                    opt => opt.MapFrom(src => src.CurrentOwner))
                .ForMember(dest => dest.SelectedNewOwner,
                    opt => opt.MapFrom(src => src.NewOwner))
                .ForMember(dest => dest.ApprovedBy,
                    opt => opt.MapFrom(src => src.ApprovedByUser))
                .ForMember(dest => dest.SupportingDocuments,
                    opt => opt.MapFrom(src => src.SupportingDocuments));
        }
    }
} 