using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<PagedResult<T>> GetPagedAsync(QueryParameters parameters);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
} 