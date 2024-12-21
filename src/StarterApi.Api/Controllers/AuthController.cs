using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Interfaces;
using StarterApi.Domain.Settings;
using StarterApi.Infrastructure.Services;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _authService = authService;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("request-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestOtp(OtpRequestDto request)
        {
            try
            {
                await _authService.RequestOtpAsync(request);
                return Ok(new { message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting OTP");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> VerifyOtp(OtpVerificationDto request)
        {
            try
            {
                var response = await _authService.VerifyOtpAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("set-tenant")]
        [Authorize]
        public async Task<ActionResult<TenantContextResponseDto>> SetTenant(SetTenantRequestDto request)
        {
            try
            {
                var response = await _authService.SetTenantAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting tenant");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            try
            {
                var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
                if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                // Generate new tokens
                var accessToken = await _jwtService.GenerateTenantTokenAsync(refreshToken.User, request.TenantId);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                // Revoke old refresh token
                await _refreshTokenRepository.RevokeAsync(request.RefreshToken);

                // Save new refresh token
                var refreshTokenEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = refreshToken.UserId,
                    ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays),
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
                };

                await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                await _refreshTokenRepository.SaveChangesAsync();

                return Ok(new RefreshTokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenDurationInMinutes)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new { message = "An error occurred while refreshing the token" });
            }
        }
    }
} 