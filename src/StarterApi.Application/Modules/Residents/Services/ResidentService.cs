using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Individuals.DTOs;
using StarterApi.Application.Modules.Individuals.Interfaces;
using StarterApi.Application.Modules.Residents.DTOs;
using StarterApi.Application.Modules.Residents.Interfaces;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Application.Modules.Users.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Residents.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _residentRepository;
        private readonly IIndividualService _individualService;
        private readonly IUnitService _unitService;
        private readonly ITenantUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<ResidentService> _logger;
        private readonly ITenantDbContext _context;

        public ResidentService(
            IResidentRepository residentRepository,
            IIndividualService individualService,
            IUnitService unitService,
            ITenantUserService userService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ILogger<ResidentService> logger,
            ITenantDbContext context)
        {
            _residentRepository = residentRepository;
            _individualService = individualService;
            _unitService = unitService;
            _userService = userService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<ResidentDto> GetByIdAsync(Guid id)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            return _mapper.Map<ResidentDto>(resident);
        }

        public async Task<IEnumerable<ResidentDto>> GetAllAsync()
        {
            var residents = await _residentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResidentDto>>(residents);
        }

        public async Task<IEnumerable<ResidentDto>> GetByUnitIdAsync(Guid unitId)
        {
            var residents = await _residentRepository.GetByUnitIdAsync(unitId);
            return _mapper.Map<IEnumerable<ResidentDto>>(residents);
        }

        public async Task<IEnumerable<ResidentDto>> GetByIndividualIdAsync(Guid individualId)
        {
            var residents = await _residentRepository.GetByIndividualIdAsync(individualId);
            return _mapper.Map<IEnumerable<ResidentDto>>(residents);
        }

        public async Task<ResidentDto> GetByUserIdAsync(Guid userId)
        {
            var resident = await _residentRepository.GetByUserIdAsync(userId);
            if (resident == null)
                throw new NotFoundException("Resident", $"User {userId}");

            return _mapper.Map<ResidentDto>(resident);
        }

        public async Task<ResidentDto> CreateAsync(CreateResidentDto dto)
        {
            _logger.LogInformation("Starting resident creation process for unit {UnitId}", dto.UnitId);

            // Validate unit availability
            var isUnitAvailable = await _residentRepository.IsUnitAvailableForResidentAsync(dto.UnitId);
            _logger.LogInformation("Unit {UnitId} availability check result: {IsAvailable}", dto.UnitId, isUnitAvailable);
            if (!isUnitAvailable)
                throw new ValidationException("Unit is not available for new residents");

            // Create individual first
            _logger.LogInformation("Creating individual for resident with email {Email}", dto.Individual.Email);
            var individualDto = await _individualService.CreateIndividualAsync(dto.Individual);
            _logger.LogInformation("Created individual with ID {IndividualId} for resident", individualDto.Id);

            // Check if individual already has active residency
            var hasActiveResidency = await _residentRepository.HasActiveResidencyAsync(individualDto.Id);
            _logger.LogInformation("Individual {IndividualId} active residency check result: {HasActiveResidency}", individualDto.Id, hasActiveResidency);
            if (hasActiveResidency)
                throw new ValidationException("Individual already has an active residency");

            // Check primary resident
            if (dto.PrimaryResident)
            {
                var hasPrimaryResident = await _residentRepository.IsPrimaryResidentExistsForUnitAsync(dto.UnitId);
                _logger.LogInformation("Unit {UnitId} primary resident check result: {HasPrimaryResident}", dto.UnitId, hasPrimaryResident);
                if (hasPrimaryResident)
                    throw new ValidationException("Unit already has a primary resident");
            }

            // Get resident role ID
            var roleId = await GetResidentRoleId();
            _logger.LogInformation("Retrieved resident role ID: {RoleId}", roleId);

            // Create user account
            var userInvitation = new UserInvitationDto
            {
                Email = individualDto.Email,
                MobileNumber = individualDto.PhoneNumber,
                FirstName = individualDto.FirstName,
                LastName = individualDto.LastName,
                RoleId = roleId
            };
            _logger.LogInformation("Inviting user with email {Email}, role {RoleId}, phone {Phone}", 
                userInvitation.Email, userInvitation.RoleId, userInvitation.MobileNumber);
            var userProfile = await _userService.InviteUserAsync(userInvitation);
            _logger.LogInformation("Created user profile with ID {UserId} for email {Email}", userProfile.Id, userProfile.Email);

            // Verify user exists in both databases
            var tenantUser = await _context.Users.FindAsync(userProfile.Id);
            if (tenantUser == null)
            {
                _logger.LogError("User {UserId} not found in tenant database after creation", userProfile.Id);
                throw new InvalidOperationException($"User {userProfile.Id} not found in tenant database after creation");
            }
            _logger.LogInformation("Verified user {UserId} exists in tenant database", userProfile.Id);

            // Create resident
            var resident = new Resident
            {
                UnitId = dto.UnitId,
                IndividualId = individualDto.Id,
                UserId = userProfile.Id,
                ResidentType = dto.ResidentType,
                RelationToOwner = dto.RelationToOwner,
                PrimaryResident = dto.PrimaryResident,
                Status = "Pending",
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                IsVerified = false
            };

            try
            {
                _logger.LogInformation("Creating resident with UserId {UserId}, IndividualId {IndividualId}, UnitId {UnitId}, Type {Type}", 
                    resident.UserId, resident.IndividualId, resident.UnitId, resident.ResidentType);
                await _residentRepository.CreateAsync(resident);
                _logger.LogInformation("Successfully created resident {ResidentId} for unit {UnitId}", resident.Id, dto.UnitId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create resident for user {UserId} in unit {UnitId}", userProfile.Id, dto.UnitId);
                throw;
            }

            return await GetByIdAsync(resident.Id);
        }

        private async Task<Guid> GetResidentRoleId()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Resident");
            if (role == null)
                throw new NotFoundException("Resident role not found. Please ensure the Resident role is created in the system.");
            return role.Id;
        }

        public async Task<ResidentDto> UpdateAsync(Guid id, UpdateResidentDto dto)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            // Validate primary resident change
            if (dto.PrimaryResident && !resident.PrimaryResident)
            {
                if (await _residentRepository.IsPrimaryResidentExistsForUnitAsync(resident.UnitId, id))
                    throw new ValidationException("Unit already has a primary resident");
            }

            // Update individual if provided
            if (dto.Individual != null)
            {
                await _individualService.UpdateIndividualAsync(resident.IndividualId, dto.Individual);
            }

            // Update resident properties
            resident.ResidentType = dto.ResidentType;
            resident.RelationToOwner = dto.RelationToOwner;
            resident.PrimaryResident = dto.PrimaryResident;
            resident.CheckOutDate = dto.CheckOutDate;
            resident.Status = dto.Status;

            await _residentRepository.UpdateAsync(resident);
            _logger.LogInformation("Updated resident {ResidentId}", id);

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            var result = await _residentRepository.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("Deleted resident {ResidentId}", id);
            }

            return result;
        }

        public async Task<ResidentDto> ApproveAsync(Guid id)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            resident.Status = "Approved";
            await _residentRepository.UpdateAsync(resident);
            _logger.LogInformation("Approved resident {ResidentId}", id);

            return await GetByIdAsync(id);
        }

        public async Task<ResidentDto> RejectAsync(Guid id, string reason)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            resident.Status = "Rejected";
            await _residentRepository.UpdateAsync(resident);
            _logger.LogInformation("Rejected resident {ResidentId}. Reason: {Reason}", id, reason);

            return await GetByIdAsync(id);
        }

        public async Task<ResidentDto> VerifyResidentAsync(Guid id)
        {
            var resident = await _residentRepository.GetByIdAsync(id);
            if (resident == null)
                throw new NotFoundException(nameof(Resident), id);

            resident.IsVerified = true;
            resident.VerifiedAt = DateTime.UtcNow;
            resident.VerifiedBy = _currentUserService.UserId;

            await _residentRepository.UpdateAsync(resident);
            _logger.LogInformation("Verified resident {ResidentId}", id);

            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<ResidentDto>> GetPendingVerificationsAsync()
        {
            var residents = await _residentRepository.GetPendingVerificationsAsync();
            return _mapper.Map<IEnumerable<ResidentDto>>(residents);
        }

        public async Task<bool> HasActiveResidencyAsync(Guid individualId)
        {
            return await _residentRepository.HasActiveResidencyAsync(individualId);
        }

        public async Task<bool> IsUnitAvailableForResidentAsync(Guid unitId)
        {
            return await _residentRepository.IsUnitAvailableForResidentAsync(unitId);
        }

        public async Task<PagedResult<ResidentListDto>> GetResidentsAsync(QueryParameters parameters)
        {
            var pagedResult = await _residentRepository.GetResidentsAsync(parameters);
            
            return new PagedResult<ResidentListDto>
            {
                Items = _mapper.Map<IEnumerable<ResidentListDto>>(pagedResult.Items),
                TotalItems = pagedResult.TotalItems,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalPages = pagedResult.TotalPages,
                HasNextPage = pagedResult.HasNextPage,
                HasPreviousPage = pagedResult.HasPreviousPage
            };
        }
    }
} 