using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Societies.DTOs;
using StarterApi.Application.Modules.Societies.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Societies.Services
{
    public class SocietyService : ISocietyService
    {
        private readonly ISocietyRepository _societyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SocietyService> _logger;

        public SocietyService(
            ISocietyRepository societyRepository,
            IMapper mapper,
            ILogger<SocietyService> logger)
        {
            _societyRepository = societyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SocietyDto> CreateSocietyAsync(CreateSocietyDto dto)
        {
            if (await _societyRepository.ExistsAsync(dto.RegistrationNumber))
                throw new InvalidOperationException($"Society with registration number {dto.RegistrationNumber} already exists");

            var society = new Society
            {
                Name = dto.Name,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                Pincode = dto.Pincode,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                RegistrationNumber = dto.RegistrationNumber,
                TotalBlocks = 0
            };

            await _societyRepository.AddAsync(society);
            await _societyRepository.SaveChangesAsync();

            return _mapper.Map<SocietyDto>(society);
        }

        public async Task<SocietyDto> GetSocietyByIdAsync(Guid id)
        {
            var society = await _societyRepository.GetByIdAsync(id);
            if (society == null)
                throw new NotFoundException($"Society with ID {id} not found");

            return _mapper.Map<SocietyDto>(society);
        }

        public async Task<SocietyDto> UpdateSocietyAsync(Guid id, UpdateSocietyDto dto)
        {
            var society = await _societyRepository.GetByIdAsync(id);
            if (society == null)
                throw new NotFoundException($"Society with ID {id} not found");

            society.Name = dto.Name;
            society.AddressLine1 = dto.AddressLine1;
            society.AddressLine2 = dto.AddressLine2;
            society.City = dto.City;
            society.State = dto.State;
            society.Country = dto.Country;
            society.Pincode = dto.Pincode;
            society.ContactNumber = dto.ContactNumber;
            society.Email = dto.Email;

            await _societyRepository.UpdateAsync(society);
            await _societyRepository.SaveChangesAsync();

            return _mapper.Map<SocietyDto>(society);
        }

        public async Task<bool> DeleteSocietyAsync(Guid id)
        {
            var society = await _societyRepository.GetByIdAsync(id);
            if (society == null)
                throw new NotFoundException($"Society with ID {id} not found");

            society.IsActive = false;
            await _societyRepository.UpdateAsync(society);
            await _societyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<SocietyListDto>> GetSocietiesAsync(QueryParameters parameters)
        {
            var pagedSocieties = await _societyRepository.GetPagedAsync(parameters);
            
            var societyDtos = _mapper.Map<IEnumerable<SocietyListDto>>(pagedSocieties.Items);
            
            return new PagedResult<SocietyListDto>
            {
                Items = societyDtos,
                TotalItems = pagedSocieties.TotalItems,
                PageNumber = pagedSocieties.PageNumber,
                PageSize = pagedSocieties.PageSize,
                TotalPages = pagedSocieties.TotalPages,
                HasNextPage = pagedSocieties.HasNextPage,
                HasPreviousPage = pagedSocieties.HasPreviousPage
            };
        }

        public async Task<bool> ExistsByRegistrationNumberAsync(string registrationNumber)
        {
            return await _societyRepository.ExistsAsync(registrationNumber);
        }
    }
}
