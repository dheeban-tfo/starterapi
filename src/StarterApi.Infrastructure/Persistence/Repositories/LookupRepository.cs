using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly ITenantDbContext _context;

    public LookupRepository(ITenantDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Individual>> GetIndividualLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Individuals.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(i =>
                i.FullName.Contains(searchTerm) ||
                i.PhoneNumber.Contains(searchTerm) ||
                i.Email.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IEnumerable<Unit>> GetUnitLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Units
            .Include(u => u.Floor)
                .ThenInclude(f => f.Block)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u =>
                u.UnitNumber.Contains(searchTerm) ||
                u.Floor.FloorName.Contains(searchTerm) ||
                u.Floor.Block.Name.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IEnumerable<Block>> GetBlockLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Blocks
            .Include(b => b.Society)
            .Where(b => b.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(b =>
                b.Name.Contains(searchTerm) ||
                b.Code.Contains(searchTerm) ||
                b.Society.Name.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IEnumerable<Floor>> GetFloorLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Floors
            .Include(f => f.Block)
            .Include(f => f.Units.Where(u => u.IsActive))
            .Where(f => f.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(f =>
                f.FloorName.Contains(searchTerm) ||
                f.Block.Name.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IEnumerable<Resident>> GetResidentLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Residents
            .Include(r => r.Individual)
            .Include(r => r.Unit)
            .Where(r => r.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(r =>
                r.Individual.FullName.Contains(searchTerm) ||
                r.Unit.UnitNumber.Contains(searchTerm) ||
                r.ResidentType.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }

    public async Task<IEnumerable<TenantUser>> GetUserLookupsAsync(string searchTerm, int maxResults)
    {
        var query = _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsActive)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u =>
                u.FirstName.Contains(searchTerm) ||
                u.LastName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.MobileNumber.Contains(searchTerm) ||
                u.Role.Name.Contains(searchTerm));
        }

        return await query
            .Take(maxResults)
            .ToListAsync();
    }
}
