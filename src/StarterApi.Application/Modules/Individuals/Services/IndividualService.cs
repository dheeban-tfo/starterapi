using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Individuals.DTOs;
using StarterApi.Application.Modules.Individuals.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Individuals.Services
{
    public class IndividualService : IIndividualService
    {
        private readonly IIndividualRepository _individualRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IndividualService> _logger;

        public IndividualService(
            IIndividualRepository individualRepository,
            IMapper mapper,
            ILogger<IndividualService> logger)
        {
            _individualRepository = individualRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IndividualDto> CreateIndividualAsync(CreateIndividualDto dto)
        {
            // Validate unique constraints
            if (await _individualRepository.ExistsByPhoneNumberAsync(dto.PhoneNumber))
                throw new InvalidOperationException($"Individual with phone number {dto.PhoneNumber} already exists");

            var individual = new Individual
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FullName = $"{dto.FirstName} {dto.LastName}".Trim(),
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Email = dto.Email,
                AlternatePhoneNumber = dto.AlternatePhoneNumber,
                IdProofType = dto.IdProofType,
                IdProofNumber = dto.IdProofNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                Pincode = dto.Pincode,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactNumber = dto.EmergencyContactNumber,
                IsVerified = false
            };

            await _individualRepository.AddAsync(individual);
            await _individualRepository.SaveChangesAsync();

            return _mapper.Map<IndividualDto>(individual);
        }

        public async Task<IndividualDto> UpdateIndividualAsync(Guid id, UpdateIndividualDto dto)
        {
            var individual = await _individualRepository.GetByIdAsync(id);
            if (individual == null)
                throw new NotFoundException($"Individual with ID {id} not found");

            // Validate unique constraints if email or phone number is changed
            if (dto.Email != individual.Email && await _individualRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException($"Individual with email {dto.Email} already exists");

            if (dto.PhoneNumber != individual.PhoneNumber && await _individualRepository.ExistsByPhoneNumberAsync(dto.PhoneNumber))
                throw new InvalidOperationException($"Individual with phone number {dto.PhoneNumber} already exists");

            individual.FirstName = dto.FirstName;
            individual.LastName = dto.LastName;
            individual.FullName = $"{dto.FirstName} {dto.LastName}";
            individual.DateOfBirth = dto.DateOfBirth;
            individual.Gender = dto.Gender;
            individual.Email = dto.Email;
            individual.PhoneNumber = dto.PhoneNumber;
            individual.AlternatePhoneNumber = dto.AlternatePhoneNumber;
            individual.AddressLine1 = dto.AddressLine1;
            individual.AddressLine2 = dto.AddressLine2;
            individual.City = dto.City;
            individual.State = dto.State;
            individual.Country = dto.Country;
            individual.Pincode = dto.Pincode;
            individual.EmergencyContactName = dto.EmergencyContactName;
            individual.EmergencyContactNumber = dto.EmergencyContactNumber;

            await _individualRepository.UpdateAsync(individual);
            await _individualRepository.SaveChangesAsync();

            return _mapper.Map<IndividualDto>(individual);
        }

        public async Task<IndividualDto> VerifyIndividualAsync(Guid id, VerifyIndividualDto dto)
        {
            var individual = await _individualRepository.GetByIdAsync(id);
            if (individual == null)
                throw new NotFoundException($"Individual with ID {id} not found");

            individual.IsVerified = dto.IsVerified;
            individual.LastVerifiedAt = DateTime.UtcNow;
            // TODO: Set VerifiedBy from current user context

            await _individualRepository.UpdateAsync(individual);
            await _individualRepository.SaveChangesAsync();

            return _mapper.Map<IndividualDto>(individual);
        }

        public async Task<bool> DeleteIndividualAsync(Guid id)
        {
            var individual = await _individualRepository.GetByIdAsync(id);
            if (individual == null)
                throw new NotFoundException($"Individual with ID {id} not found");

            individual.IsActive = false;
            await _individualRepository.UpdateAsync(individual);
            await _individualRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IndividualDto> GetIndividualByIdAsync(Guid id)
        {
            var individual = await _individualRepository.GetByIdAsync(id);
            if (individual == null)
                throw new NotFoundException($"Individual with ID {id} not found");

            return _mapper.Map<IndividualDto>(individual);
        }

        public async Task<PagedResult<IndividualDto>> GetIndividualsAsync(QueryParameters parameters)
        {
            var pagedIndividuals = await _individualRepository.GetPagedAsync(parameters);
            
            var individualDtos = _mapper.Map<IEnumerable<IndividualDto>>(pagedIndividuals.Items);
            
            return new PagedResult<IndividualDto>
            {
                Items = individualDtos,
                TotalItems = pagedIndividuals.TotalItems,
                PageNumber = pagedIndividuals.PageNumber,
                PageSize = pagedIndividuals.PageSize,
                TotalPages = pagedIndividuals.TotalPages,
                HasNextPage = pagedIndividuals.HasNextPage,
                HasPreviousPage = pagedIndividuals.HasPreviousPage
            };
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _individualRepository.ExistsByEmailAsync(email);
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _individualRepository.ExistsByPhoneNumberAsync(phoneNumber);
        }

        public async Task<bool> ExistsByIdProofAsync(string idProofType, string idProofNumber)
        {
            return await _individualRepository.ExistsByIdProofAsync(idProofType, idProofNumber);
        }
    }
}
