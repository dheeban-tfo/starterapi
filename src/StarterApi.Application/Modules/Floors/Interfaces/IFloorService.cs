using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Floors.DTOs;

namespace StarterApi.Application.Modules.Floors.Interfaces
{
    public interface IFloorService
    {
        Task<FloorDto> CreateFloorAsync(CreateFloorDto dto);
        Task<FloorDto> UpdateFloorAsync(Guid id, UpdateFloorDto dto);
        Task<bool> DeleteFloorAsync(Guid id);
        Task<FloorDto> GetFloorByIdAsync(Guid id);
        Task<PagedResult<FloorListDto>> GetFloorsAsync(QueryParameters parameters);
        Task<bool> ExistsByNumberAsync(int number, Guid blockId);
    }
}
