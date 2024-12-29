using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Entities;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class FacilityBookingRepository : IFacilityBookingRepository
    {
        private readonly ITenantDbContext _context;

        public FacilityBookingRepository(ITenantDbContext context)
        {
            _context = context;
        }

        public async Task<FacilityBooking> GetByIdAsync(Guid id)
        {
            return await _context.FacilityBookings
                .Include(b => b.Facility)
                .Include(b => b.Resident)
                    .ThenInclude(r => r.Individual)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<PagedResult<FacilityBooking>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.FacilityBookings
                .Include(b => b.Facility)
                .Include(b => b.Resident)
                    .ThenInclude(r => r.Individual)
                .Where(b => b.IsActive);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Facility.Name.ToLower().Contains(searchTerm) ||
                    b.Resident.Individual.FirstName.ToLower().Contains(searchTerm) ||
                    b.Resident.Individual.LastName.ToLower().Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<FacilityBooking>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public async Task<PagedResult<FacilityBooking>> GetByFacilityAsync(
            Guid facilityId,
            QueryParameters parameters)
        {
            var query = _context.FacilityBookings
                .Include(b => b.Facility)
                .Include(b => b.Resident)
                    .ThenInclude(r => r.Individual)
                .Where(b => b.IsActive && b.FacilityId == facilityId);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Resident.Individual.FirstName.ToLower().Contains(searchTerm) ||
                    b.Resident.Individual.LastName.ToLower().Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<FacilityBooking>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public async Task<PagedResult<FacilityBooking>> GetByResidentAsync(
            Guid residentId,
            QueryParameters parameters)
        {
            var query = _context.FacilityBookings
                .Include(b => b.Facility)
                .Include(b => b.Resident)
                    .ThenInclude(r => r.Individual)
                .Where(b => b.IsActive && b.ResidentId == residentId);

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Facility.Name.ToLower().Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<FacilityBooking>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public async Task<bool> HasOverlappingBookingsAsync(
            Guid facilityId,
            DateTime date,
            TimeSpan startTime,
            TimeSpan endTime,
            Guid? excludeBookingId = null)
        {
            var query = _context.FacilityBookings
                .Where(b => b.IsActive &&
                           b.FacilityId == facilityId &&
                           b.Date == date &&
                           b.BookingStatus != "Cancelled" &&
                           ((b.StartTime <= startTime && b.EndTime > startTime) ||
                            (b.StartTime < endTime && b.EndTime >= endTime) ||
                            (b.StartTime >= startTime && b.EndTime <= endTime)));

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsActiveBookingAsync(Guid id)
        {
            return await _context.FacilityBookings
                .AnyAsync(b => b.Id == id && 
                              b.IsActive && 
                              b.BookingStatus != "Cancelled");
        }

        public async Task<FacilityBooking> AddAsync(FacilityBooking booking)
        {
            await _context.FacilityBookings.AddAsync(booking);
            return booking;
        }

        public async Task<FacilityBooking> UpdateAsync(FacilityBooking booking)
        {
            _context.FacilityBookings.Update(booking);
            return booking;
        }

        public async Task<bool> DeleteAsync(FacilityBooking booking)
        {
            booking.IsActive = false;
            _context.FacilityBookings.Update(booking);
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Resident> GetResidentAsync(Guid residentId)
        {
            return await _context.Residents
                .Include(r => r.Individual)
                .Include(r => r.Unit)
                .FirstOrDefaultAsync(r => r.Id == residentId && r.IsActive);
        }

        public async Task<IEnumerable<FacilityBooking>> GetBookingsByDateAsync(Guid facilityId, DateTime date)
        {
            return await _context.FacilityBookings
                .Where(b => b.FacilityId == facilityId &&
                           b.Date.Date == date.Date &&
                           b.IsActive)
                .ToListAsync();
        }
    }
} 