using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StarterApi.Application.Common.Exceptions;


public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    public TenantResolutionMiddleware(
        RequestDelegate next,
        ILogger<TenantResolutionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantService tenantService,
        ITenantProvider tenantProvider)
    {
        _logger.LogInformation("Starting tenant resolution for path: {Path}", context.Request.Path);

        var allClaims = context.User.Claims.ToList();
        // _logger.LogInformation("All claims in tenant middleware: {@Claims}", //enable this line to see all claims
        //     allClaims.Select(c => new { c.Type, c.Value }));

        if (context.User.Identity?.IsAuthenticated != true)
        {
            _logger.LogWarning("Request is not authenticated");
            await _next(context);
            return;
        }

        var tokenType = context.User.FindFirst("token_type")?.Value;
        _logger.LogInformation("Token type from claims: {TokenType}", tokenType);

        if (tokenType != "tenant_token" && tokenType != "base_token")
        {
            _logger.LogWarning("Invalid token type: {TokenType}", tokenType);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "Valid token required" });
            return;
        }

        if (tokenType == "tenant_token")
        {
            var tenantId = context.User.FindFirst("tenant_id")?.Value;
            _logger.LogInformation("Tenant ID from claims: {TenantId}", tenantId);

            if (string.IsNullOrEmpty(tenantId) || !Guid.TryParse(tenantId, out Guid parsedTenantId))
            {
                _logger.LogWarning("Invalid tenant ID in token: {TenantId}", tenantId);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid tenant token" });
                return;
            }

            try
            {
                var tenant = await tenantService.GetTenantByIdAsync(parsedTenantId);
                if (tenant != null)
                {
                    tenantProvider.SetCurrentTenantId(parsedTenantId);
                    _logger.LogInformation("Successfully set tenant context: {TenantId}", parsedTenantId);
                }
                else
                {
                    _logger.LogWarning("Tenant not found: {TenantId}", parsedTenantId);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid tenant" });
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving tenant");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "Error resolving tenant" });
                return;
            }
        }

        await _next(context);
    }
} 