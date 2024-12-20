using Microsoft.EntityFrameworkCore;

using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly RootDbContext _context;

        public TenantRepository(RootDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant> GetByIdAsync(Guid id)
        {
            return await _context.Tenants.FindAsync(id);
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
            return tenant;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 