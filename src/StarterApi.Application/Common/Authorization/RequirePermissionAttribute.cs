using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Authorization;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    private readonly ILogger<RequirePermissionAttribute> _logger;

    public RequirePermissionAttribute(string permission) : base($"Permission_{permission}")
    {
        // _logger = LoggerFactory.Create(builder => 
        //     builder.AddConsole()).CreateLogger<RequirePermissionAttribute>();
        
       // _logger.LogInformation("Creating permission requirement for: {Permission}", permission);
    }
} 