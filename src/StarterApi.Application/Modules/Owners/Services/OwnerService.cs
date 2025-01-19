using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Owners.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Owners.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerService> _logger;

        public OwnerService(
            IOwnerRepository ownerRepository,
            IDocumentRepository documentRepository,
            IMapper mapper,
            ILogger<OwnerService> logger)
        {
            _ownerRepository = ownerRepository;
            _documentRepository = documentRepository;
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
            var owner = _mapper.Map<Owner>(dto);
            await _ownerRepository.AddAsync(owner);
            await _ownerRepository.SaveChangesAsync();

            return _mapper.Map<OwnerDto>(owner);
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

        public async Task<List<DocumentDto>> GetOwnerDocumentsAsync(Guid ownerId)
        {
            var documents = await _ownerRepository.GetOwnerDocumentsAsync(ownerId);
            return _mapper.Map<List<DocumentDto>>(documents);
        }

        public async Task<DocumentDto> AddOwnerDocumentAsync(Guid ownerId, Guid documentId)
        {
            await _ownerRepository.AddOwnerDocumentAsync(ownerId, documentId);
            await _ownerRepository.SaveChangesAsync();

            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
                throw new NotFoundException($"Document with ID {documentId} not found");

            return _mapper.Map<DocumentDto>(document);
        }

        public async Task<bool> RemoveOwnerDocumentAsync(Guid ownerId, Guid documentId)
        {
            await _ownerRepository.RemoveOwnerDocumentAsync(ownerId, documentId);
            await _ownerRepository.SaveChangesAsync();
            return true;
        }
    }
} 