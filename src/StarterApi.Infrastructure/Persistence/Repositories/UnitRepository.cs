using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Units.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ITenantDbContext _context;

        public UnitRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> GetByIdAsync(Guid id)
        {
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                        .ThenInclude(b => b.Society)
                .Include(u => u.CurrentOwner)
                    .ThenInclude(o => o.Individual)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<PagedResult<Unit>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                        .ThenInclude(b => b.Society)
                .Include(u => u.CurrentOwner)
                    .ThenInclude(o => o.Individual)
                .AsQueryable();

            // Apply Search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply Filters
            query = query.ApplyFiltering(parameters.Filters);

            // Apply Sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Get total count
            var totalItems = await query.CountAsync();

            // Apply Pagination
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Unit>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<Unit> AddAsync(Unit unit)
        {
            await _context.Units.AddAsync(unit);
            return unit;
        }

        public async Task<Unit> UpdateAsync(Unit unit)
        {
            _context.Units.Update(unit);
            return unit;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Unit> GetByNumberAsync(string number, Guid floorId)
        {
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                        .ThenInclude(b => b.Society)
                .Include(u => u.CurrentOwner)
                    .ThenInclude(o => o.Individual)
                .FirstOrDefaultAsync(u => u.UnitNumber == number && u.FloorId == floorId && u.IsActive);
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

        public async Task<Owner> GetOwnerByIdAsync(Guid id)
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Individual> GetIndividualByIdAsync(Guid id)
        {
            return await _context.Individuals
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Owner> GetOwnerByIndividualIdAsync(Guid individualId)
        {
            return await _context.Owners
                .Include(o => o.Individual)
                .FirstOrDefaultAsync(o => o.IndividualId == individualId);
        }

        public async Task<Owner> AddOwnerAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            return owner;
        }
    }
} 