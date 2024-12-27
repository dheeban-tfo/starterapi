using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Entities;


namespace StarterApi.Repositories
{
    public class DocumentRepository
    {
        private readonly ITenantDbContext _context;

        public DocumentRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetByUnitAsync(Guid unitId)
        {
            return await _context.Documents
                .Include(d => d.DocumentCategory)
                .Include(d => d.Unit)
                    .ThenInclude(u => u.Floor)
                        .ThenInclude(f => f.Block)
                            .ThenInclude(b => b.Society)
                .Include(d => d.Versions)
                .Where(d => d.UnitId == unitId && d.IsActive)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
    }
} 