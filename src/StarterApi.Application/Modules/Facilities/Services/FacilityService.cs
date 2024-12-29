using System.ComponentModel.DataAnnotations;
using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Domain.Enums;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _repository;
        private readonly IFacilityBookingRuleRepository _bookingRuleRepository;
        private readonly IMapper _mapper;

        public FacilityService(
            IFacilityRepository repository,
            IFacilityBookingRuleRepository bookingRuleRepository,
            IMapper mapper)
        {
            _repository = repository;
            _bookingRuleRepository = bookingRuleRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<FacilityListDto>> GetFacilitiesAsync(QueryParameters parameters)
        {
            var facilities = await _repository.GetPagedAsync(parameters);
            
            // Get all active bookings counts in a single query
            var activeBookingsCounts = await _repository.GetActiveBookingsCountForFacilitiesAsync(
                facilities.Items.Select(f => f.Id).ToList());

            var dtos = facilities.Items.Select(f =>
            {
                var dto = _mapper.Map<FacilityListDto>(f);
                dto.ActiveBookingsCount = activeBookingsCounts.GetValueOrDefault(f.Id, 0);
                return dto;
            });

            return new PagedResult<FacilityListDto>
            {
                Items = dtos.ToList(),
                TotalItems = facilities.TotalItems,
                PageNumber = facilities.PageNumber,
                PageSize = facilities.PageSize,
                TotalPages = facilities.TotalPages,
                HasNextPage = facilities.HasNextPage,
                HasPreviousPage = facilities.HasPreviousPage
            };
        }

        public async Task<FacilityDto> GetByIdAsync(Guid id)
        {
            var facility = await _repository.GetByIdWithDetailsAsync(id);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), id);
            }

            var dto = _mapper.Map<FacilityDto>(facility);
            dto.ActiveBookingsCount = await _repository.GetActiveBookingsCountAsync(id);
            return dto;
        }

        public async Task<FacilityDto> CreateAsync(CreateFacilityDto dto)
        {
            if (await _repository.ExistsAsync(dto.Name))
            {
                throw new ValidationException($"A facility with name '{dto.Name}' already exists.");
            }

            var facility = _mapper.Map<Facility>(dto);
            facility = await _repository.AddAsync(facility);

            // Create default booking rules
            var defaultRule = new FacilityBookingRule
            {
                FacilityId = facility.Id,
                StartTime = new TimeSpan(9, 0, 0), // 9:00 AM
                EndTime = new TimeSpan(21, 0, 0),  // 9:00 PM
                MaxDurationMinutes = 120,          // 2 hours
                MinDurationMinutes = 30,           // 30 minutes
                MinAdvanceBookingHours = 1,        // 1 hour
                MaxAdvanceBookingDays = 30,        // 30 days
                AllowMultipleBookings = false,
                MaxBookingsPerDay = 1,
                MaxActiveBookings = 1,
                RequireApproval = true,
                CancellationPolicy = "Cancellations must be made at least 24 hours in advance.",
                IsActive = true
            };

            await _bookingRuleRepository.AddAsync(defaultRule);

            var result = _mapper.Map<FacilityDto>(facility);
            result.ActiveBookingsCount = 0;
            return result;
        }

        public async Task<FacilityDto> UpdateAsync(Guid id, UpdateFacilityDto dto)
        {
            var facility = await _repository.GetByIdAsync(id);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), id);
            }

            if (await _repository.ExistsAsync(dto.Name, id))
            {
                throw new ValidationException($"A facility with name '{dto.Name}' already exists.");
            }

            _mapper.Map(dto, facility);
            await _repository.UpdateAsync(facility);
            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var facility = await _repository.GetByIdAsync(id);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), id);
            }

            return await _repository.DeleteAsync(facility);
        }

        public Task<IEnumerable<FacilityTypeDto>> GetFacilityTypesAsync()
        {
            var types = Enum.GetValues(typeof(FacilityType))
                .Cast<FacilityType>()
                .Select(t => new FacilityTypeDto
                {
                    Id = ((int)t).ToString(),
                    Name = t.ToString(),
                    Description = GetFacilityTypeDescription(t)
                });

            return Task.FromResult(types);
        }

        public Task<IEnumerable<FacilityStatusDto>> GetFacilityStatusTypesAsync()
        {
            var statuses = Enum.GetValues(typeof(FacilityStatus))
                .Cast<FacilityStatus>()
                .Select(s => new FacilityStatusDto
                {
                    Id = ((int)s).ToString(),
                    Name = s.ToString(),
                    Description = GetFacilityStatusDescription(s)
                });

            return Task.FromResult(statuses);
        }

        private string GetFacilityTypeDescription(FacilityType type)
        {
            return type switch
            {
                FacilityType.SwimmingPool => "Swimming pool facility for residents",
                FacilityType.Gym => "Fitness center with exercise equipment",
                FacilityType.FunctionHall => "Multi-purpose hall for events and gatherings",
                FacilityType.TennisCourt => "Tennis court facility",
                FacilityType.BadmintonCourt => "Badminton court facility",
                FacilityType.TableTennis => "Table tennis facility",
                FacilityType.Playground => "Children's playground area",
                FacilityType.Garden => "Garden and landscaped area",
                FacilityType.Library => "Reading and study area",
                FacilityType.GameRoom => "Indoor games and recreation room",
                FacilityType.BBQPit => "Barbecue and outdoor cooking area",
                _ => "Other facility type"
            };
        }

        private string GetFacilityStatusDescription(FacilityStatus status)
        {
            return status switch
            {
                FacilityStatus.Active => "Facility is available for use",
                FacilityStatus.UnderMaintenance => "Facility is currently under maintenance",
                FacilityStatus.TemporarilyClosed => "Facility is temporarily closed",
                FacilityStatus.Inactive => "Facility is not available for use",
                _ => "Unknown status"
            };
        }

        public async Task<bool> TemporaryCloseAsync(Guid id)
        {
            var facility = await _repository.GetByIdAsync(id);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), id);
            }

            facility.Status = FacilityStatus.TemporarilyClosed;
            await _repository.UpdateAsync(facility);
            return true;
        }

        public async Task<bool> ReopenAsync(Guid id)
        {
            var facility = await _repository.GetByIdAsync(id);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), id);
            }

            facility.Status = FacilityStatus.Active;
            await _repository.UpdateAsync(facility);
            return true;
        }

        public async Task<IEnumerable<FacilityListDto>> GetBySocietyIdAsync(Guid societyId)
        {
            var facilities = await _repository.GetBySocietyIdAsync(societyId);
            var dtos = facilities.Select(async f =>
            {
                var dto = _mapper.Map<FacilityListDto>(f);
                dto.ActiveBookingsCount = await _repository.GetActiveBookingsCountAsync(f.Id);
                return dto;
            });

            return await Task.WhenAll(dtos);
        }
    }
} 