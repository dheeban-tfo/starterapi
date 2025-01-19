using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Owners.DTOs;
using StarterApi.Application.Modules.Units.DTOs;

namespace StarterApi.Application.Modules.Units.Interfaces
{
    public interface IUnitService
    {
        Task<UnitDto> CreateUnitAsync(CreateUnitDto dto);
        Task<UnitDto> UpdateUnitAsync(Guid id, UpdateUnitDto dto);
        Task<bool> DeleteUnitAsync(Guid id);
        Task<UnitDto> GetUnitByIdAsync(Guid id);
        Task<PagedResult<UnitListDto>> GetUnitsAsync(QueryParameters parameters);
        Task<bool> ExistsByNumberAsync(string number, Guid floorId);
        Task<PagedResult<OwnershipHistoryListDto>> GetUnitOwnershipHistoryAsync(Guid unitId, QueryParameters parameters);
        Task<UnitBulkImportResultDto> BulkImportAsync(IEnumerable<UnitBulkImportDto> units);
    }
}
