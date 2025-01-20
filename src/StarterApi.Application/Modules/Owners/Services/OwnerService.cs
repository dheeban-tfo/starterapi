using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Owners.Interfaces;
using StarterApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StarterApi.Application.Modules.Owners.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ITenantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerService> _logger;

        public OwnerService(
            IOwnerRepository ownerRepository,
            ITenantDbContext context,
            IMapper mapper,
            ILogger<OwnerService> logger)
        {
            _ownerRepository = ownerRepository;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<OwnerListDto>> GetOwnersAsync(QueryParameters parameters)
        {
            var pagedOwners = await _ownerRepository.GetPagedAsync(parameters);
            var ownerDtos = _mapper.Map<IEnumerable<OwnerListDto>>(pagedOwners.Items);

            return new PagedResult<OwnerListDto>
            {
                Items = ownerDtos,
                TotalItems = pagedOwners.TotalItems,
                PageNumber = pagedOwners.PageNumber,
                PageSize = pagedOwners.PageSize,
                TotalPages = pagedOwners.TotalPages,
                HasNextPage = pagedOwners.HasNextPage,
                HasPreviousPage = pagedOwners.HasPreviousPage
            };
        }

        public async Task<OwnerDto> GetByIdAsync(Guid id)
        {
            var owner = await _ownerRepository.GetByIdWithDetailsAsync(id);
            if (owner == null)
                throw new NotFoundException($"Owner with ID {id} not found");

            return _mapper.Map<OwnerDto>(owner);
        }

        public async Task<OwnerDto> CreateAsync(CreateOwnerDto dto)
        {
            // First create the individual
            var individual = _mapper.Map<Individual>(dto.Individual);
            individual.FullName = $"{individual.FirstName} {individual.LastName}";
            
            await _context.Individuals.AddAsync(individual);
            await _context.SaveChangesAsync();

            // Then create the owner
            var owner = new Owner
            {
                IndividualId = individual.Id,
                Individual = individual,
                OwnershipType = dto.OwnershipType,
                OwnershipPercentage = dto.OwnershipPercentage,
                OwnershipStartDate = dto.OwnershipStartDate,
                OwnershipDocumentNumber = dto.OwnershipDocumentNumber,
                Status = "Active"
            };

            await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveChangesAsync();

            // Assign units if any
            if (dto.UnitIds?.Any() == true)
            {
                var units = await _context.Units
                    .Where(u => dto.UnitIds.Contains(u.Id))
                    .ToListAsync();

                foreach (var unit in units)
                {
                    unit.CurrentOwnerId = owner.Id;
                }

                await _context.SaveChangesAsync();
            }

            // Fetch the complete owner with details for the response
            var createdOwner = await _ownerRepository.GetByIdWithDetailsAsync(owner.Id);
            return _mapper.Map<OwnerDto>(createdOwner);
        }

        public async Task<OwnerDto> UpdateAsync(Guid id, UpdateOwnerDto dto)
        {
            var owner = await _ownerRepository.GetByIdWithDetailsAsync(id);
            if (owner == null)
                throw new NotFoundException($"Owner with ID {id} not found");

            _mapper.Map(dto, owner);
            await _ownerRepository.UpdateAsync(owner);
            await _ownerRepository.SaveChangesAsync();

            return _mapper.Map<OwnerDto>(owner);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var owner = await _ownerRepository.GetByIdWithDetailsAsync(id);
            if (owner == null)
                throw new NotFoundException($"Owner with ID {id} not found");

            await _ownerRepository.DeleteAsync(id);
            await _ownerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<OwnershipHistoryListDto>> GetOwnerHistoryAsync(Guid ownerId, QueryParameters parameters)
        {
            // Since there's no direct method for getting owner history in the repository,
            // we'll get the owner with details and map the history
            var owner = await _ownerRepository.GetByIdWithDetailsAsync(ownerId);
            if (owner == null)
                throw new NotFoundException($"Owner with ID {ownerId} not found");

            // Assuming the Owner entity has a History property
            var historyDtos = _mapper.Map<IEnumerable<OwnershipHistoryListDto>>(owner.OwnershipHistory);

            // Create a paged result manually since we don't have a repository method for this
            return new PagedResult<OwnershipHistoryListDto>
            {
                Items = historyDtos,
                TotalItems = owner.OwnershipHistory?.Count ?? 0,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling((owner.OwnershipHistory?.Count ?? 0) / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling((owner.OwnershipHistory?.Count ?? 0) / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public async Task<PagedResult<OwnershipHistoryListDto>> GetUnitOwnershipHistoryAsync(Guid unitId, QueryParameters parameters)
        {
            // Get all owners for the unit
            var owners = await _ownerRepository.GetByUnitIdAsync(unitId);
            
            // Collect all ownership history
            var allHistory = new List<OwnershipHistory>();
            foreach (var owner in owners)
            {
                if (owner.OwnershipHistory != null)
                {
                    allHistory.AddRange(owner.OwnershipHistory);
                }
            }

            // Order by date and take the requested page
            var orderedHistory = allHistory.OrderByDescending(h => h.TransferDate)
                                         .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                         .Take(parameters.PageSize);

            var historyDtos = _mapper.Map<IEnumerable<OwnershipHistoryListDto>>(orderedHistory);

            return new PagedResult<OwnershipHistoryListDto>
            {
                Items = historyDtos,
                TotalItems = allHistory.Count,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(allHistory.Count / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(allHistory.Count / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }
    }
} 