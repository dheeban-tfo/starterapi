using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StarterApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Extensions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUserTenantRepository _userTenantRepository;
    private readonly ITenantProvider _tenantProvider;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;

    public PermissionAuthorizationHandler(
        IUserTenantRepository userTenantRepository,
        ITenantProvider tenantProvider,
        ILogger<PermissionAuthorizationHandler> logger)
    {
        _userTenantRepository = userTenantRepository;
        _tenantProvider = tenantProvider;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        _logger.LogInformation("Starting permission check for: {Permission}", requirement.Permission);
        
        var user = context.User;
        // _logger.LogInformation("User claims: {@Claims}", 
        //     user.Claims.Select(c => new { c.Type, c.Value })); //enable this line to see all claims

        var userId = user.GetUserId();
        var tenantId = _tenantProvider.GetCurrentTenantId();

        _logger.LogInformation("Checking permission. UserId: {UserId}, TenantId: {TenantId}, Permission: {Permission}", 
            userId, tenantId, requirement.Permission);

        if (!tenantId.HasValue)
        {
            _logger.LogWarning("No tenant context found during permission check");
            context.Fail();
            return;
        }

        try
        {
            var permissions = await _userTenantRepository.GetUserPermissionsAsync(userId, tenantId.Value);
            //_logger.LogInformation("User permissions: {@Permissions}", permissions);

            if (permissions.Contains(requirement.Permission))
            {
                _logger.LogInformation("Permission granted: {Permission}", requirement.Permission);
                context.Succeed(requirement);
                return;
            }

            _logger.LogWarning("Permission denied. Required: {Permission}, Available: {@Permissions}", 
                requirement.Permission, permissions);
            context.Fail();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking permissions for user {UserId} in tenant {TenantId}", 
                userId, tenantId);
            context.Fail();
        }
    }
} 