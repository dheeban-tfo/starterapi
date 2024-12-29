using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Facilities.DTOs;
using StarterApi.Application.Modules.Facilities.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacilityBookingsController : ControllerBase
    {
        private readonly IFacilityBookingService _bookingService;
        private readonly ILogger<FacilityBookingsController> _logger;

        public FacilityBookingsController(
            IFacilityBookingService bookingService,
            ILogger<FacilityBookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        /// <summary>
        /// Get a paged list of facility bookings
        /// </summary>
        [HttpGet]
        [RequirePermission(Permissions.FacilityBookings.View)]
        public async Task<ActionResult<PagedResult<FacilityBookingListDto>>> GetBookings(
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsAsync(parameters);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility bookings");
                return StatusCode(500, "An error occurred while retrieving facility bookings");
            }
        }

        /// <summary>
        /// Get facility booking by ID
        /// </summary>
        [HttpGet("{id}")]
        [RequirePermission(Permissions.FacilityBookings.View)]
        public async Task<ActionResult<FacilityBookingDto>> GetBooking(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking == null)
                    return NotFound();

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility booking");
                return StatusCode(500, "An error occurred while retrieving the facility booking");
            }
        }

        /// <summary>
        /// Create a new facility booking
        /// </summary>
        [HttpPost]
        [RequirePermission(Permissions.FacilityBookings.Create)]
        public async Task<ActionResult<FacilityBookingDto>> CreateBooking(CreateFacilityBookingDto dto)
        {
            try
            {
                var isAvailable = await _bookingService.CheckAvailabilityAsync(
                    dto.FacilityId, dto.Date, dto.StartTime, dto.EndTime);

                if (!isAvailable)
                    return BadRequest("The facility is not available for the selected time slot");

                var booking = await _bookingService.CreateBookingAsync(dto);
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating facility booking");
                return StatusCode(500, "An error occurred while creating the facility booking");
            }
        }

        /// <summary>
        /// Cancel an existing booking
        /// </summary>
        [HttpPost("{id}/cancel")]
        [RequirePermission(Permissions.FacilityBookings.Cancel)]
        public async Task<ActionResult> CancelBooking(Guid id)
        {
            try
            {
                var result = await _bookingService.CancelBookingAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling facility booking");
                return StatusCode(500, "An error occurred while canceling the facility booking");
            }
        }

        /// <summary>
        /// Get bookings for a specific facility
        /// </summary>
        [HttpGet("facility/{facilityId}")]
        [RequirePermission(Permissions.FacilityBookings.View)]
        public async Task<ActionResult<PagedResult<FacilityBookingListDto>>> GetBookingsByFacility(
            Guid facilityId,
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByFacilityAsync(facilityId, parameters);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving facility bookings");
                return StatusCode(500, "An error occurred while retrieving facility bookings");
            }
        }

        /// <summary>
        /// Get bookings for a specific resident
        /// </summary>
        [HttpGet("resident/{residentId}")]
        [RequirePermission(Permissions.FacilityBookings.View)]
        public async Task<ActionResult<PagedResult<FacilityBookingListDto>>> GetBookingsByResident(
            Guid residentId,
            [FromQuery] QueryParameters parameters)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByResidentAsync(residentId, parameters);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving resident bookings");
                return StatusCode(500, "An error occurred while retrieving resident bookings");
            }
        }

        /// <summary>
        /// Check facility availability for a time slot
        /// </summary>
        [HttpGet("check-availability")]
        [RequirePermission(Permissions.FacilityBookings.View)]
        public async Task<ActionResult<bool>> CheckAvailability(
            [FromQuery] Guid facilityId,
            [FromQuery] DateTime date,
            [FromQuery] TimeSpan startTime,
            [FromQuery] TimeSpan endTime)
        {
            try
            {
                var isAvailable = await _bookingService.CheckAvailabilityAsync(
                    facilityId, date, startTime, endTime);
                return Ok(isAvailable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking facility availability");
                return StatusCode(500, "An error occurred while checking facility availability");
            }
        }
    }
} 