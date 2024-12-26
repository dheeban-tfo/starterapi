using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Common.Interfaces;
using StarterApi.Domain.Constants;

namespace StarterApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("individuals")]
    [RequirePermission(Permissions.Individuals.View)]
    public async Task<ActionResult<IEnumerable<IndividualLookupDto>>> GetIndividuals(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100) // Limit maximum results to 100
        };

        var results = await _lookupService.GetIndividualLookupsAsync(request);
        return Ok(results); // Always return 200 OK with the results (empty or not)
    }

    [HttpGet("units")]
    [RequirePermission(Permissions.Units.View)]
    public async Task<ActionResult<IEnumerable<UnitLookupDto>>> GetUnits(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100) // Limit maximum results to 100
        };

        var results = await _lookupService.GetUnitLookupsAsync(request);
        return Ok(results); // Always return 200 OK with the results (empty or not)
    }

    [HttpGet("blocks")]
    [RequirePermission(Permissions.Blocks.View)]
    public async Task<ActionResult<IEnumerable<BlockLookupDto>>> GetBlocks(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100)
        };

        var results = await _lookupService.GetBlockLookupsAsync(request);
        return Ok(results);
    }

    [HttpGet("floors")]
    [RequirePermission(Permissions.Floors.View)]
    public async Task<ActionResult<IEnumerable<FloorLookupDto>>> GetFloors(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100)
        };

        var results = await _lookupService.GetFloorLookupsAsync(request);
        return Ok(results);
    }

    [HttpGet("residents")]
    [RequirePermission(Permissions.Residents.View)]
    public async Task<ActionResult<IEnumerable<ResidentLookupDto>>> GetResidents(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100)
        };

        var results = await _lookupService.GetResidentLookupsAsync(request);
        return Ok(results);
    }

    [HttpGet("users")]
    [RequirePermission(Permissions.Users.View)]
    public async Task<ActionResult<IEnumerable<UserLookupDto>>> GetUsers(
        [FromQuery] string searchTerm,
        [FromQuery] int maxResults = 10)
    {
        var request = new LookupRequestDto
        {
            SearchTerm = searchTerm,
            MaxResults = Math.Min(maxResults, 100)
        };

        var results = await _lookupService.GetUserLookupsAsync(request);
        return Ok(results);
    }
}
