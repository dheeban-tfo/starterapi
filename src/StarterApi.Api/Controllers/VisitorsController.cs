using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Modules.Visitors.DTOs;
using StarterApi.Application.Modules.Visitors.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitorsController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public VisitorsController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        [RequirePermission(Permissions.Visitors.View)]
        public async Task<ActionResult<PagedResult<VisitorListDto>>> GetVisitors([FromQuery] QueryParameters parameters)
        {
            var visitors = await _visitorService.GetVisitorsAsync(parameters);
            return Ok(visitors);
        }

        [HttpGet("upcoming")]
        [RequirePermission(Permissions.Visitors.View)]
        public async Task<ActionResult<PagedResult<VisitorListDto>>> GetUpcomingVisitors([FromQuery] QueryParameters parameters)
        {
            var visitors = await _visitorService.GetUpcomingVisitorsAsync(parameters);
            return Ok(visitors);
        }

        [HttpGet("past")]
        [RequirePermission(Permissions.Visitors.View)]
        public async Task<ActionResult<PagedResult<VisitorListDto>>> GetPastVisitors([FromQuery] QueryParameters parameters)
        {
            var visitors = await _visitorService.GetPastVisitorsAsync(parameters);
            return Ok(visitors);
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.Visitors.View)]
        public async Task<ActionResult<VisitorDto>> GetVisitor(Guid id)
        {
            var visitor = await _visitorService.GetByIdAsync(id);
            return Ok(visitor);
        }

        [HttpPost]
        [RequirePermission(Permissions.Visitors.Create)]
        public async Task<ActionResult<VisitorDto>> CreateVisitor(CreateVisitorDto dto)
        {
            var visitor = await _visitorService.CreateVisitorAsync(dto);
            return CreatedAtAction(nameof(GetVisitor), new { id = visitor.Id }, visitor);
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.Visitors.Edit)]
        public async Task<ActionResult<VisitorDto>> UpdateVisitor(Guid id, UpdateVisitorDto dto)
        {
            var visitor = await _visitorService.UpdateVisitorAsync(id, dto);
            return Ok(visitor);
        }

        [HttpPut("{id}/status")]
        [RequirePermission(Permissions.Visitors.Approve)]
        public async Task<ActionResult<VisitorDto>> UpdateVisitorStatus(Guid id, UpdateVisitorStatusDto dto)
        {
            var visitor = await _visitorService.UpdateVisitorStatusAsync(id, dto);
            return Ok(visitor);
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.Visitors.Delete)]
        public async Task<ActionResult> DeleteVisitor(Guid id)
        {
            await _visitorService.DeleteVisitorAsync(id);
            return NoContent();
        }
    }
} 