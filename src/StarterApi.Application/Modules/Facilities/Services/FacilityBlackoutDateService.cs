using System.ComponentModel.DataAnnotations;
using AutoMapper;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Facilities.Services
{
    public class FacilityBlackoutDateService : IFacilityBlackoutDateService
    {
        private readonly IFacilityBlackoutDateRepository _repository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;

        public FacilityBlackoutDateService(
            IFacilityBlackoutDateRepository repository,
            IFacilityRepository facilityRepository,
            IMapper mapper)
        {
            _repository = repository;
            _facilityRepository = facilityRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<FacilityBlackoutDateDto>> GetPagedAsync(Guid facilityId, QueryParameters parameters)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            var blackoutDates = await _repository.GetPagedAsync(facilityId, parameters);
            return new PagedResult<FacilityBlackoutDateDto>
            {
                Items = _mapper.Map<IEnumerable<FacilityBlackoutDateDto>>(blackoutDates.Items),
                TotalItems = blackoutDates.TotalItems,
                PageNumber = blackoutDates.PageNumber,
                PageSize = blackoutDates.PageSize
            };
        }

        public async Task<FacilityBlackoutDateDto> GetByIdAsync(Guid id)
        {
            var blackoutDate = await _repository.GetByIdAsync(id);
            if (blackoutDate == null)
            {
                throw new NotFoundException(nameof(FacilityBlackoutDate), id);
            }

            return _mapper.Map<FacilityBlackoutDateDto>(blackoutDate);
        }

        public async Task<IEnumerable<FacilityBlackoutDateDto>> GetByDateRangeAsync(Guid facilityId, DateTime startDate, DateTime endDate)
        {
            var facility = await _facilityRepository.GetByIdAsync(facilityId);
            if (facility == null)
            {
                throw new NotFoundException(nameof(Facility), facilityId);
            }

            var blackoutDates = await _repository.GetByDateRangeAsync(facilityId, startDate, endDate);
            return _mapper.Map<IEnumerable<FacilityBlackoutDateDto>>(blackoutDates);
        }

        public async Task<FacilityBlackoutDateDto> UpdateAsync(Guid id, UpdateFacilityBlackoutDateDto dto)
        {
            var blackoutDate = await _repository.GetByIdAsync(id);
            if (blackoutDate == null)
            {
                throw new NotFoundException(nameof(FacilityBlackoutDate), id);
            }

            // Validate date range
            if (dto.EndDate < dto.StartDate)
            {
                throw new ValidationException("End date cannot be earlier than start date");
            }

            _mapper.Map(dto, blackoutDate);
            blackoutDate = await _repository.UpdateAsync(blackoutDate);
            return _mapper.Map<FacilityBlackoutDateDto>(blackoutDate);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var blackoutDate = await _repository.GetByIdAsync(id);
            if (blackoutDate == null)
            {
                throw new NotFoundException(nameof(FacilityBlackoutDate), id);
            }

            return await _repository.DeleteAsync(blackoutDate);
        }
    }
} 