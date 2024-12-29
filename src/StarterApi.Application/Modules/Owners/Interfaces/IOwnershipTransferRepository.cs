using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Owners.Interfaces
{
    public interface IOwnershipTransferRepository : IBaseRepository<OwnershipTransferRequest>
    {
        new Task<PagedResult<OwnershipTransferRequest>> GetPagedAsync(QueryParameters parameters);
        Task<OwnershipTransferRequest> GetByIdWithDetailsAsync(Guid id);
        Task<bool> HasPendingTransferAsync(Guid unitId);
        Task<bool> IsValidTransferRequestAsync(Guid unitId, Guid currentOwnerId);
    }
} 