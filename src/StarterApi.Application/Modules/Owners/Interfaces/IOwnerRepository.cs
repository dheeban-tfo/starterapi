using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Owners.Interfaces
{
    public interface IOwnerRepository : IBaseRepository<Owner>
    {
        new Task<PagedResult<Owner>> GetPagedAsync(QueryParameters parameters);
        Task<Owner> GetByIdWithDetailsAsync(Guid id);
        Task<List<Owner>> GetByUnitIdAsync(Guid unitId);
        Task<bool> HasActiveOwnershipAsync(Guid unitId, Guid? excludeOwnerId = null);
    }
} 