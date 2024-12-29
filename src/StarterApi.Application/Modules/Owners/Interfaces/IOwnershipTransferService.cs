using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.DTOs;

namespace StarterApi.Application.Modules.Owners.Interfaces
{
    public interface IOwnershipTransferService
    {
        Task<PagedResult<OwnershipTransferListDto>> GetTransferRequestsAsync(QueryParameters parameters);
        Task<OwnershipTransferDto> GetTransferRequestByIdAsync(Guid id);
        Task<OwnershipTransferDto> CreateTransferRequestAsync(CreateOwnershipTransferDto dto);
        Task<OwnershipTransferDto> ApproveTransferRequestAsync(Guid id);
        Task<OwnershipTransferDto> RejectTransferRequestAsync(Guid id, UpdateOwnershipTransferStatusDto dto);
    }
} 