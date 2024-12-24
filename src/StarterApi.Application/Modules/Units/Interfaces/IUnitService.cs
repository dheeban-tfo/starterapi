using StarterApi.Application.Modules.Units.DTOs;

namespace StarterApi.Application.Modules.Units.Interfaces
{
    public interface IUnitService
    {
        Task<UnitDto> CreateUnitAsync(CreateUnitDto dto);
        Task<UnitDto> UpdateUnitAsync(Guid id, UpdateUnitDto dto);
        Task<bool> DeleteUnitAsync(Guid id);
        Task<UnitDto> GetUnitByIdAsync(Guid id);
        Task<IEnumerable<UnitDto>> GetAllUnitsAsync();
        Task<IEnumerable<UnitDto>> GetUnitsByFloorAsync(Guid floorId);
        Task<bool> ExistsByNumberAsync(Guid floorId, string unitNumber);
    }
}
