using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Documents.DTOs;
using StarterApi.Application.Modules.Owners.DTOs;

namespace StarterApi.Application.Modules.Owners.Interfaces
{
    public interface IOwnerService
    {
        Task<PagedResult<OwnerListDto>> GetOwnersAsync(QueryParameters parameters);
        Task<OwnerDto> GetByIdAsync(Guid id);
        Task<OwnerDto> CreateAsync(CreateOwnerDto dto);
        Task<OwnerDto> UpdateAsync(Guid id, UpdateOwnerDto dto);
        Task<bool> DeleteAsync(Guid id);
        
        // History related methods
        Task<PagedResult<OwnershipHistoryListDto>> GetOwnerHistoryAsync(Guid ownerId, QueryParameters parameters);
        Task<PagedResult<OwnershipHistoryListDto>> GetUnitOwnershipHistoryAsync(Guid unitId, QueryParameters parameters);
    }
} 