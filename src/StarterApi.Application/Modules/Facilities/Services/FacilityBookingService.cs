using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityBookingService : IFacilityBookingService
    {
        private readonly IFacilityBookingRepository _repository;
        private readonly IMapper _mapper;

        public FacilityBookingService(
            IFacilityBookingRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<FacilityBookingListDto>> GetBookingsAsync(QueryParameters parameters)
        {
            var bookings = await _repository.GetPagedAsync(parameters);
            
            return new PagedResult<FacilityBookingListDto>
            {
                Items = _mapper.Map<IEnumerable<FacilityBookingListDto>>(bookings.Items),
                TotalItems = bookings.TotalItems,
                PageNumber = bookings.PageNumber,
                PageSize = bookings.PageSize,
                TotalPages = bookings.TotalPages,
                HasNextPage = bookings.HasNextPage,
                HasPreviousPage = bookings.HasPreviousPage
            };
        }

        public async Task<FacilityBookingDto> GetByIdAsync(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
                throw new NotFoundException(nameof(FacilityBooking), id);

            return _mapper.Map<FacilityBookingDto>(booking);
        }

        public async Task<FacilityBookingDto> CreateBookingAsync(CreateFacilityBookingDto dto)
        {
            // Check if resident exists
            var resident = await _repository.GetResidentAsync(dto.ResidentId);
            if (resident == null)
                throw new ValidationException($"Resident with ID {dto.ResidentId} does not exist.");

            // Check for overlapping bookings
            // var hasOverlap = await _repository.HasOverlappingBookingsAsync(
            //     dto.FacilityId,
            //     dto.Date,
            //     dto.StartTime,
            //     dto.EndTime);

            // if (hasOverlap)
            //     throw new ValidationException("The selected time slot is not available.");

            var booking = new FacilityBooking
            {
                FacilityId = dto.FacilityId,
                ResidentId = dto.ResidentId,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                BookingStatus = "Confirmed",
                PaymentStatus = "Pending",
                SpecialRequest = dto.SpecialRequest,
                IsActive = true
            };

            await _repository.AddAsync(booking);
            await _repository.SaveChangesAsync();

            return await GetByIdAsync(booking.Id);
        }

        public async Task<bool> CancelBookingAsync(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
                throw new NotFoundException(nameof(FacilityBooking), id);

            if (!await _repository.ExistsActiveBookingAsync(id))
                throw new ValidationException("Booking is already cancelled or inactive.");

            booking.BookingStatus = "Cancelled";
            await _repository.UpdateAsync(booking);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckAvailabilityAsync(
            Guid facilityId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            return !await _repository.HasOverlappingBookingsAsync(
                facilityId,
                date,
                startTime,
                endTime);
        }

        public async Task<PagedResult<FacilityBookingListDto>> GetBookingsByFacilityAsync(
            Guid facilityId,
            QueryParameters parameters)
        {
            var bookings = await _repository.GetByFacilityAsync(facilityId, parameters);
            
            return new PagedResult<FacilityBookingListDto>
            {
                Items = _mapper.Map<IEnumerable<FacilityBookingListDto>>(bookings.Items),
                TotalItems = bookings.TotalItems,
                PageNumber = bookings.PageNumber,
                PageSize = bookings.PageSize,
                TotalPages = bookings.TotalPages,
                HasNextPage = bookings.HasNextPage,
                HasPreviousPage = bookings.HasPreviousPage
            };
        }

        public async Task<PagedResult<FacilityBookingListDto>> GetBookingsByResidentAsync(
            Guid residentId,
            QueryParameters parameters)
        {
            var bookings = await _repository.GetByResidentAsync(residentId, parameters);
            
            return new PagedResult<FacilityBookingListDto>
            {
                Items = _mapper.Map<IEnumerable<FacilityBookingListDto>>(bookings.Items),
                TotalItems = bookings.TotalItems,
                PageNumber = bookings.PageNumber,
                PageSize = bookings.PageSize,
                TotalPages = bookings.TotalPages,
                HasNextPage = bookings.HasNextPage,
                HasPreviousPage = bookings.HasPreviousPage
            };
        }
    }
} 