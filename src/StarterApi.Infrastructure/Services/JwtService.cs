using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StarterApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using StarterApi.Application.Interfaces;
using StarterApi.Domain.Settings;
using StarterApi.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;



public class JwtService : IJwtService
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<JwtService> _logger;

    public JwtService(ITokenService tokenService, ILogger<JwtService> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public string GenerateBaseToken(User user)
    {
        _logger.LogInformation("Generating base token for user: {UserId}", user.Id);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email ?? user.MobileNumber),
            new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
            new Claim("UserType", user.UserType.ToString()),
            new Claim("token_type", "base_token")
        };

        _logger.LogDebug("Claims for token: {@Claims}", claims);

        return _tokenService.GenerateToken(claims);
    }

    public string GenerateRefreshToken()
    {
        return _tokenService.GenerateRefreshToken();
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        return _tokenService.ValidateToken(token);
    }

    public async Task<string> GenerateTenantTokenAsync(User user, Guid tenantId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email ?? user.MobileNumber),
            new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
            new Claim("UserType", user.UserType.ToString()),
            new Claim("tenant_id", tenantId.ToString()),
            new Claim("token_type", "tenant_token")
        };

        return _tokenService.GenerateToken(claims);
    }
} 