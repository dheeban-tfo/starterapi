using Microsoft.AspNetCore.Http;
using StarterApi.Application.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;

public class JwtAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtAuthorizationMiddleware> _logger;
    private readonly ITokenService _tokenService;

    public JwtAuthorizationMiddleware(
        RequestDelegate next,
        ILogger<JwtAuthorizationMiddleware> logger,
        ITokenService tokenService)
    {
        _next = next;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        _logger.LogInformation("Starting JWT authorization for path: {Path}", context.Request.Path);
        
        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var principal = _tokenService.ValidateToken(token);
                
                // Map claims to standard types
                var claims = new List<Claim>();
                foreach (var claim in principal.Claims)
                {
                    // Map to standard claim types if needed
                    if (claim.Type.Contains("nameidentifier"))
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                    }
                    claims.Add(claim); // Keep original claim too
                }

                var identity = new ClaimsIdentity(claims, "Bearer");
                context.User = new ClaimsPrincipal(identity);

                // _logger.LogInformation("Claims after mapping: {@Claims}", 
                //     context.User.Claims.Select(c => new { c.Type, c.Value })); //enable this line to see all claims
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating JWT token");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid token" });
                return;
            }
        }

        await _next(context);
    }

    private bool RequiresTenantToken(PathString path)
    {
        // Add paths that require tenant token
        var tenantPaths = new[] { "/api/Users", "/api/Reports" };
        return tenantPaths.Any(p => path.StartsWithSegments(p));
    }
} 