using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Residents.DTOs;

namespace StarterApi.Application.Modules.Residents.Interfaces
{
    public interface IResidentService
    {
        Task<ResidentDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ResidentDto>> GetAllAsync();
        Task<PagedResult<ResidentDto>> GetResidentsAsync(QueryParameters parameters);
        Task<IEnumerable<ResidentDto>> GetByUnitIdAsync(Guid unitId);
        Task<IEnumerable<ResidentDto>> GetByIndividualIdAsync(Guid individualId);
        Task<ResidentDto> GetByUserIdAsync(Guid userId);
        Task<ResidentDto> CreateAsync(CreateResidentDto dto);
        Task<ResidentDto> UpdateAsync(Guid id, UpdateResidentDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ResidentDto> ApproveAsync(Guid id);
        Task<ResidentDto> RejectAsync(Guid id, string reason);
        Task<ResidentDto> VerifyResidentAsync(Guid id);
        Task<IEnumerable<ResidentDto>> GetPendingVerificationsAsync();
        Task<bool> HasActiveResidencyAsync(Guid individualId);
        Task<bool> IsUnitAvailableForResidentAsync(Guid unitId);
    }
} 