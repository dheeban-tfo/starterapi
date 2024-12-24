using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Blocks.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly TenantDbContext _context;

        public BlockRepository(TenantDbContext context)
        {
            _context = context;
        }

        public async Task<Block> GetByIdAsync(Guid id)
        {
            return await _context.Blocks.FindAsync(id);
        }

        public async Task<PagedResult<Block>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Blocks.AsQueryable();

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
            _context.Entry(block).State = EntityState.Modified;
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