using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityBookingRuleService : IFacilityBookingRuleService
    {
        private readonly IFacilityBookingRuleRepository _repository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityBlackoutDateRepository _blackoutDateRepository;
        private readonly IMapper _mapper;
        private readonly IFacilityBookingRepository _bookingRepository;

        public FacilityBookingRuleService(
            IFacilityBookingRuleRepository repository,
            IFacilityRepository facilityRepository,
            IFacilityBlackoutDateRepository blackoutDateRepository,
            IMapper mapper,
            IFacilityBookingRepository bookingRepository)
        {
            _repository = repository;
            _facilityRepository = facilityRepository;
            _blackoutDateRepository = blackoutDateRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
        }

        public async Task<FacilityBookingRuleDto> GetByFacilityIdAsync(Guid facilityId)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            var rule = await _repository.GetByFacilityIdAsync(facilityId);
            return _mapper.Map<FacilityBookingRuleDto>(rule);
        }

        public async Task<FacilityBookingRuleDto> UpdateAsync(Guid facilityId, UpdateFacilityBookingRuleDto dto)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            var rule = await _repository.GetByFacilityIdAsync(facilityId);
            if (rule == null)
            {
                rule = new FacilityBookingRule { FacilityId = facilityId };
                rule = await _repository.AddAsync(rule);
            }

            _mapper.Map(dto, rule);
            rule = await _repository.UpdateAsync(rule);
            return _mapper.Map<FacilityBookingRuleDto>(rule);
        }

        public async Task<IEnumerable<TimeSlotDto>> GetAvailableSlotsAsync(Guid facilityId, DateTime date)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            var rule = await _repository.GetByFacilityIdAsync(facilityId);
            if (rule == null)
            {
                throw new NotFoundException("Booking rules not found for facility", facilityId);
            }

            // Check if date is within blackout dates
            var blackoutDates = await _blackoutDateRepository.GetByDateRangeAsync(facilityId, date.Date, date.Date);
            if (blackoutDates.Any())
            {
                return new List<TimeSlotDto>
                {
                    new TimeSlotDto
                    {
                        StartTime = rule.StartTime.ToString(@"hh\:mm"),
                        EndTime = rule.EndTime.ToString(@"hh\:mm"),
                        IsAvailable = false,
                        UnavailabilityReason = "Facility is not available on this date"
                    }
                };
            }

            // Get all existing bookings for this date
            var existingBookings = await _bookingRepository.GetBookingsByDateAsync(facilityId, date);

            // Generate time slots based on rules
            var slots = new List<TimeSlotDto>();
            var currentTime = rule.StartTime;
            var slotDuration = TimeSpan.FromMinutes(rule.MinDurationMinutes);

            while (currentTime.Add(slotDuration) <= rule.EndTime)
            {
                var slotEndTime = currentTime.Add(slotDuration);
                var isSlotAvailable = true;
                var unavailabilityReason = "";

                // Check if this slot overlaps with any existing booking
                foreach (var booking in existingBookings)
                {
                    if (booking.BookingStatus == "Cancelled")
                        continue;

                    if ((currentTime >= booking.StartTime && currentTime < booking.EndTime) ||
                        (slotEndTime > booking.StartTime && slotEndTime <= booking.EndTime) ||
                        (currentTime <= booking.StartTime && slotEndTime >= booking.EndTime))
                    {
                        isSlotAvailable = false;
                        unavailabilityReason = "Time slot is already booked";
                        break;
                    }
                }

                slots.Add(new TimeSlotDto
                {
                    StartTime = currentTime.ToString(@"hh\:mm"),
                    EndTime = slotEndTime.ToString(@"hh\:mm"),
                    IsAvailable = isSlotAvailable,
                    UnavailabilityReason = unavailabilityReason
                });

                currentTime = currentTime.Add(slotDuration);
            }

            return slots;
        }
    }
} 