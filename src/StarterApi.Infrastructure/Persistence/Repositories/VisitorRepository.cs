using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Visitors.Interfaces;
using StarterApi.Domain.Entities;
using StarterApi.Application.Common.Extensions;
using StarterApi.Application.Common.Interfaces;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly ITenantDbContext _context;

        public VisitorRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Visitor>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Visitors
                .Include(v => v.Resident)
                .Where(v => v.IsActive);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(v => 
                    v.VisitorName.Contains(parameters.SearchTerm) ||
                    v.PurposeOfVisit.Contains(parameters.SearchTerm));
            }

            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<Visitor> GetByIdAsync(Guid id)
        {
            return await _context.Visitors
                .Include(v => v.Resident)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<Visitor> CreateAsync(Visitor visitor)
        {
            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();
            return visitor;
        }

        public async Task<Visitor> UpdateAsync(Visitor visitor)
        {
            _context.Visitors.Update(visitor);
            await _context.SaveChangesAsync();
            return visitor;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null) return false;

            visitor.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Visitors.AnyAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<PagedResult<Visitor>> GetUpcomingVisitorsAsync(QueryParameters parameters)
        {
            var today = DateTime.Today;
            var query = _context.Visitors
                .Include(v => v.Resident)
                .Where(v => v.IsActive && v.ExpectedVisitDate >= today);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(v => 
                    v.VisitorName.Contains(parameters.SearchTerm) ||
                    v.PurposeOfVisit.Contains(parameters.SearchTerm));
            }

            return await query.ToPagedResultAsync(parameters);
        }

        public async Task<PagedResult<Visitor>> GetPastVisitorsAsync(QueryParameters parameters)
        {
            var today = DateTime.Today;
            var query = _context.Visitors
                .Include(v => v.Resident)
                .Where(v => v.IsActive && v.ExpectedVisitDate < today);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(v => 
                    v.VisitorName.Contains(parameters.SearchTerm) ||
                    v.PurposeOfVisit.Contains(parameters.SearchTerm));
            }

            return await query.ToPagedResultAsync(parameters);
        }
    }
} 