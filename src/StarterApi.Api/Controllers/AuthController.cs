using Microsoft.AspNetCore.Mvc;

using StarterApi.Application.Common.Exceptions;

namespace StarterApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("request-otp")]
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
    }
} 