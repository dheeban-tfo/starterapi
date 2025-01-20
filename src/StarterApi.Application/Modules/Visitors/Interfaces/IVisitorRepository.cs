using System;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Modules.Visitors.Interfaces
{
    public interface IVisitorRepository
    {
        Task<PagedResult<Visitor>> GetPagedAsync(QueryParameters parameters);
        Task<Visitor> GetByIdAsync(Guid id);
        Task<Visitor> CreateAsync(Visitor visitor);
        Task<Visitor> UpdateAsync(Visitor visitor);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedResult<Visitor>> GetUpcomingVisitorsAsync(QueryParameters parameters);
        Task<PagedResult<Visitor>> GetPastVisitorsAsync(QueryParameters parameters);
    }
} 