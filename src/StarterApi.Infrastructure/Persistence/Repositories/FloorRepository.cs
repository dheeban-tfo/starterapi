using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ITenantDbContext _context;

        public FloorRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Floor> GetByIdAsync(Guid id)
        {
            return await _context.Floors.FindAsync(id);
        }

        public async Task<PagedResult<Floor>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Floors.AsQueryable();

            // Apply Search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply Filters
            query = query.ApplyFiltering(parameters.Filters);

            // Apply Sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return Paged Result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<Floor> AddAsync(Floor floor)
        {
            await _context.Floors.AddAsync(floor);
            return floor;
        }

        public async Task<Floor> UpdateAsync(Floor floor)
        {
            ((DbContext)_context).Entry(floor).State = EntityState.Modified;
            return floor;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Floor> GetByNumberAsync(int number, Guid blockId)
        {
            return await _context.Floors
                .FirstOrDefaultAsync(f => f.FloorNumber == number && f.BlockId == blockId);
        }

        public async Task<bool> ExistsAsync(int number, Guid blockId)
        {
            return await _context.Floors
                .AnyAsync(f => f.FloorNumber == number && f.BlockId == blockId);
        }

        public async Task<int> GetFloorCountByBlockAsync(Guid blockId)
        {
            return await _context.Floors
                .CountAsync(f => f.BlockId == blockId);
        }
    }
} 