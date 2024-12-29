using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Owners.Interfaces
{
    public interface IOwnershipHistoryRepository : IBaseRepository<OwnershipHistory>
    {
        Task<PagedResult<OwnershipHistory>> GetPagedByOwnerIdAsync(Guid ownerId, QueryParameters parameters);
        Task<PagedResult<OwnershipHistory>> GetPagedByUnitIdAsync(Guid unitId, QueryParameters parameters);
        Task<OwnershipHistory> GetByIdWithDetailsAsync(Guid id);
    }
} 