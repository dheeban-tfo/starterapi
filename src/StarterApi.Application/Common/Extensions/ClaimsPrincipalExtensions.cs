using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace StarterApi.Application.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        private static readonly ILogger _logger = 
            LoggerFactory.Create(builder => builder.AddConsole())
                .CreateLogger("ClaimsPrincipalExtensions");

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            _logger.LogInformation("Looking for user ID in claims: {@Claims}", 
                principal.Claims.Select(c => new { c.Type, c.Value }));

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) 
                ?? principal.FindFirst("sub")  
                ?? principal.FindFirst("nameid"); 

            if (userIdClaim == null)
            {
                _logger.LogError("No user ID claim found in: {@ClaimTypes}", 
                    principal.Claims.Select(c => c.Type));
                throw new UnauthorizedAccessException("User ID claim not found");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                _logger.LogError("Invalid user ID format: {Value}", userIdClaim.Value);
                throw new UnauthorizedAccessException("Invalid user ID format");
            }

            return userId;
        }
    }
} 