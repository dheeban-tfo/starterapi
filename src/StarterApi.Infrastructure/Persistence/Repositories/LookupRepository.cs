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

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchTermLower = searchTerm.ToLower();
            query = query.Where(i => 
                i.FirstName.ToLower().Contains(searchTermLower) ||
                i.LastName.ToLower().Contains(searchTermLower) ||
                i.PhoneNumber.Contains(searchTerm)
            );
        }

        return await query
            .OrderBy(i => i.FirstName)
            .Take(maxResults)
            .ToListAsync();
    }
}
