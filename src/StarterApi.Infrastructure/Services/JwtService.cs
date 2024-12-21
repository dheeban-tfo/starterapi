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



public class JwtService : IJwtService
{
    private readonly ITokenService _tokenService;

    public JwtService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public string GenerateBaseToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("token_type", "base_token")
        };

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
        throw new NotImplementedException("Use ITenantTokenService for tenant-specific operations");
    }
} 