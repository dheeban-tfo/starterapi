using Microsoft.AspNetCore.Http;
using StarterApi.Application.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;


public class JwtAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService;

    public JwtAuthorizationMiddleware(RequestDelegate next, IJwtService jwtService)
    {
        _next = next;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (token != null)
        {
            try
            {
                var principal = _jwtService.ValidateToken(token);
                var tokenType = principal.FindFirst("token_type")?.Value;

                // Ensure tenant-specific endpoints have tenant token
                if (RequiresTenantToken(context.Request.Path) && tokenType != "tenant_token")
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { message = "Tenant-specific token required" });
                    return;
                }

                context.User = principal;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
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