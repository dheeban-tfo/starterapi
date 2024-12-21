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
        var tenantIdHeader = context.Request.Headers["X-TenantId"].FirstOrDefault();

        if (!string.IsNullOrEmpty(tenantIdHeader) && Guid.TryParse(tenantIdHeader, out Guid tenantId))
        {
            try
            {
                // Verify tenant exists and is active
                var tenant = await tenantService.GetTenantByIdAsync(tenantId);
                if (tenant != null)
                {
                    tenantProvider.SetCurrentTenantId(tenantId);
                    _logger.LogInformation("Tenant resolved: {TenantId}", tenantId);
                }
                else
                {
                    _logger.LogWarning("Invalid tenant ID provided: {TenantId}", tenantId);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = "Invalid tenant ID" });
                    return;
                }
            }
            catch (NotFoundException)
            {
                _logger.LogWarning("Tenant not found: {TenantId}", tenantId);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid tenant ID" });
                return;
            }
        }

        await _next(context);
    }
} 