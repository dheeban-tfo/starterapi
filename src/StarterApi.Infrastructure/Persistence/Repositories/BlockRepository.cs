using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly ITenantDbContext _context;

        public BlockRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<Block> GetByIdAsync(Guid id)
        {
            return await _context.Blocks
                .Include(b => b.Society)
                .Include(b => b.Floors.Where(f => f.IsActive))
                    .ThenInclude(f => f.Units.Where(u => u.IsActive))
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<PagedResult<Block>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Blocks
                .Include(b => b.Society)
                .Include(b => b.Floors.Where(f => f.IsActive))
                .Where(b => b.IsActive)
                .AsQueryable();

            // Apply Search
            query = query.ApplySearch(parameters.SearchTerm);

            // Apply Filters
            query = query.ApplyFiltering(parameters.Filters);

            // Apply Sorting
            query = query.ApplySort(parameters.SortBy, parameters.IsDescending);

            // Return Paged Result
            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<Block> AddAsync(Block block)
        {
            await _context.Blocks.AddAsync(block);
            return block;
        }

        public async Task<Block> UpdateAsync(Block block)
        {
            ((DbContext)_context).Entry(block).State = EntityState.Modified;
            return block;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Block> GetByCodeAsync(string code, Guid societyId)
        {
            return await _context.Blocks
                .FirstOrDefaultAsync(b => b.Code == code && b.SocietyId == societyId);
        }

        public async Task<bool> ExistsAsync(string code, Guid societyId)
        {
            return await _context.Blocks
                .AnyAsync(b => b.Code == code && b.SocietyId == societyId);
        }

        public async Task<int> GetBlockCountBySocietyAsync(Guid societyId)
        {
            return await _context.Blocks
                .CountAsync(b => b.SocietyId == societyId);
        }
    }
} 