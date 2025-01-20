using System;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Visitors.DTOs;

namespace StarterApi.Application.Modules.Visitors.Interfaces
{
    public interface IVisitorService
    {
        Task<PagedResult<VisitorListDto>> GetVisitorsAsync(QueryParameters parameters);
        Task<VisitorDto> GetByIdAsync(Guid id);
        Task<VisitorDto> CreateVisitorAsync(CreateVisitorDto dto);
        Task<VisitorDto> UpdateVisitorAsync(Guid id, UpdateVisitorDto dto);
        Task<bool> DeleteVisitorAsync(Guid id);
        Task<VisitorDto> UpdateVisitorStatusAsync(Guid id, UpdateVisitorStatusDto dto);
        Task<PagedResult<VisitorListDto>> GetUpcomingVisitorsAsync(QueryParameters parameters);
        Task<PagedResult<VisitorListDto>> GetPastVisitorsAsync(QueryParameters parameters);
    }
} 