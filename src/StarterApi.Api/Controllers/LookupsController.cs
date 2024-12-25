using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarterApi.Application.Common.Models;
using StarterApi.Application.Common.Interfaces;

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
    [Authorize(Policy = "Permission_Individuals.View")]
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
        return Ok(results);
    }
}
