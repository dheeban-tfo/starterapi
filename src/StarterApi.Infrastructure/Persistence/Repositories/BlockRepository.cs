using Microsoft.EntityFrameworkCore;
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
            return await _context.Blocks
                .Include(b => b.Society)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Block>> GetAllAsync()
        {
            return await _context.Blocks
                .Include(b => b.Society)
                .ToListAsync();
        }

        public async Task<IEnumerable<Block>> GetBySocietyIdAsync(Guid societyId)
        {
            return await _context.Blocks
                .Include(b => b.Society)
                .Where(b => b.SocietyId == societyId)
                .ToListAsync();
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

        public async Task<bool> ExistsAsync(Guid societyId, string code)
        {
            return await _context.Blocks
                .AnyAsync(b => b.SocietyId == societyId && b.Code == code);
        }

        public async Task<int> GetBlockCountBySocietyAsync(Guid societyId)
        {
            return await _context.Blocks
                .CountAsync(b => b.SocietyId == societyId);
        }
    }
} 