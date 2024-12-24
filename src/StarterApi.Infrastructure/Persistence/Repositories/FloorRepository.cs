using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Modules.Floors.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FloorRepository : IFloorRepository
    {
        private readonly TenantDbContext _context;

        public FloorRepository(TenantDbContext context)
        {
            _context = context;
        }

        public async Task<Floor> GetByIdAsync(Guid id)
        {
            return await _context.Floors
                .Include(f => f.Block)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Floor>> GetAllAsync()
        {
            return await _context.Floors
                .Include(f => f.Block)
                .ToListAsync();
        }

        public async Task<IEnumerable<Floor>> GetByBlockIdAsync(Guid blockId)
        {
            return await _context.Floors
                .Include(f => f.Block)
                .Where(f => f.BlockId == blockId)
                .OrderBy(f => f.FloorNumber)
                .ToListAsync();
        }

        public async Task<Floor> AddAsync(Floor floor)
        {
            await _context.Floors.AddAsync(floor);
            return floor;
        }

        public async Task<Floor> UpdateAsync(Floor floor)
        {
            _context.Entry(floor).State = EntityState.Modified;
            return floor;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid blockId, int floorNumber)
        {
            return await _context.Floors
                .AnyAsync(f => f.BlockId == blockId && f.FloorNumber == floorNumber);
        }

        public async Task<int> GetFloorCountByBlockAsync(Guid blockId)
        {
            return await _context.Floors
                .CountAsync(f => f.BlockId == blockId);
        }

        public async Task<Floor> GetByBlockAndNumberAsync(Guid blockId, int floorNumber)
        {
            return await _context.Floors
                .Include(f => f.Block)
                .FirstOrDefaultAsync(f => f.BlockId == blockId && f.FloorNumber == floorNumber);
        }
    }
} 