using Microsoft.EntityFrameworkCore;
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
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                .Include(u => u.CurrentOwner)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                .Include(u => u.CurrentOwner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetByFloorIdAsync(Guid floorId)
        {
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                .Include(u => u.CurrentOwner)
                .Where(u => u.FloorId == floorId)
                .OrderBy(u => u.UnitNumber)
                .ToListAsync();
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

        public async Task<bool> ExistsAsync(Guid floorId, string unitNumber)
        {
            return await _context.Units
                .AnyAsync(u => u.FloorId == floorId && u.UnitNumber == unitNumber);
        }

        public async Task<int> GetUnitCountByFloorAsync(Guid floorId)
        {
            return await _context.Units
                .CountAsync(u => u.FloorId == floorId);
        }

        public async Task<Unit> GetByFloorAndNumberAsync(Guid floorId, string unitNumber)
        {
            return await _context.Units
                .Include(u => u.Floor)
                    .ThenInclude(f => f.Block)
                .Include(u => u.CurrentOwner)
                .FirstOrDefaultAsync(u => u.FloorId == floorId && u.UnitNumber == unitNumber);
        }

        public async Task<decimal> GetTotalMaintenanceByFloorAsync(Guid floorId)
        {
            return await _context.Units
                .Where(u => u.FloorId == floorId)
                .SumAsync(u => u.MonthlyMaintenanceFee);
        }
    }
} 