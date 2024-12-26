using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Domain.Entities;

namespace StarterApi.Application.Modules.Residents.Interfaces
{
    public interface IResidentRepository
    {
        Task<Resident> GetByIdAsync(Guid id);
        Task<IEnumerable<Resident>> GetAllAsync();
        Task<PagedResult<Resident>> GetResidentsAsync(QueryParameters parameters);
        Task<IEnumerable<Resident>> GetByUnitIdAsync(Guid unitId);
        Task<IEnumerable<Resident>> GetByIndividualIdAsync(Guid individualId);
        Task<Resident> GetByUserIdAsync(Guid userId);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> IsPrimaryResidentExistsForUnitAsync(Guid unitId, Guid? excludeResidentId = null);
        Task<Resident> CreateAsync(Resident resident);
        Task<Resident> UpdateAsync(Resident resident);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Resident>> GetPendingVerificationsAsync();
        Task<bool> HasActiveResidencyAsync(Guid individualId);
        Task<bool> IsUnitAvailableForResidentAsync(Guid unitId);
    }
} 