using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly TenantDbContext _context;

        public UnitRepository(TenantDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> GetByIdAsync(Guid id)
        {
            return await _context.Units.FindAsync(id);
        }

        public async Task<PagedResult<Unit>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Units.AsQueryable();

            // Apply Search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply Filters
            query = query.ApplyFiltering(parameters.Filters);

            // Apply Sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return Paged Result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<Unit> AddAsync(Unit unit)
        {
            await _context.Units.AddAsync(unit);
            return unit;
        }

        public async Task<Unit> UpdateAsync(Unit unit)
        {
            _context.Entry(unit).State = EntityState.Modified;
            return unit;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Unit> GetByNumberAsync(string number, Guid floorId)
        {
            return await _context.Units
                .FirstOrDefaultAsync(u => u.UnitNumber == number && u.FloorId == floorId);
        }

        public async Task<bool> ExistsAsync(string number, Guid floorId)
        {
            return await _context.Units
                .AnyAsync(u => u.UnitNumber == number && u.FloorId == floorId);
        }

        public async Task<int> GetUnitCountByFloorAsync(Guid floorId)
        {
            return await _context.Units
                .CountAsync(u => u.FloorId == floorId);
        }
    }
} 