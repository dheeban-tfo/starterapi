using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using StarterApi.Application.Modules.Users.DTOs;
using StarterApi.Domain.Entities;
using StarterApi.Application.Interfaces;
using Microsoft.Extensions.Options;
using StarterApi.Domain.Settings;

namespace StarterApi.Application.Modules.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IUserTenantRepository _userTenantRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(
            IOtpRepository otpRepository,
            IUserTenantRepository userTenantRepository,
            ITenantProvider tenantProvider,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<AuthService> logger,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _otpRepository = otpRepository;
            _userTenantRepository = userTenantRepository;
            _tenantProvider = tenantProvider;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<bool> RequestOtpAsync(OtpRequestDto request)
        {
            _logger.LogInformation("Requesting OTP for mobile: {MobileNumber}", request.MobileNumber);

            // Find user-tenant mapping
            var userTenant = await _userTenantRepository.GetByMobileNumberAsync(request.MobileNumber);
            if (userTenant == null)
            {
                _logger.LogWarning("No tenant found for mobile: {MobileNumber}", request.MobileNumber);
                throw new UnauthorizedException("User not found in any tenant");
            }

            // Set tenant context for the request
            _tenantProvider.SetCurrentTenantId(userTenant.TenantId);

            var otpRequest = new OtpRequest
            {
                MobileNumber = request.MobileNumber,
                OtpCode = "111000", // Mock OTP code
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false,
                UserId = userTenant.UserId
            };

            await _otpRepository.CreateAsync(otpRequest);
            await _otpRepository.SaveChangesAsync();

            _logger.LogInformation("OTP created for user in tenant: {TenantId}", userTenant.TenantId);
            return true;
        }

        public async Task<AuthResponseDto> VerifyOtpAsync(OtpVerificationDto request)
        {
            _logger.LogInformation("Verifying OTP for mobile: {MobileNumber}", request.MobileNumber);

            // Find user-tenant mapping
            var userTenant = await _userTenantRepository.GetByMobileNumberAsync(request.MobileNumber);
            if (userTenant == null)
            {
                _logger.LogWarning("No tenant found for mobile: {MobileNumber}", request.MobileNumber);
                throw new UnauthorizedException("User not found in any tenant");
            }

            // Set tenant context for the request
            _tenantProvider.SetCurrentTenantId(userTenant.TenantId);

            // Validate OTP
            var isValid = await _otpRepository.ValidateOtpAsync(request.MobileNumber, request.OtpCode);
            if (!isValid)
            {
                _logger.LogWarning("Invalid OTP for mobile: {MobileNumber}", request.MobileNumber);
                throw new UnauthorizedException("Invalid OTP");
            }

            // Get user details from tenant database
            var user = await _userRepository.GetByMobileNumberAsync(request.MobileNumber);
            if (user == null)
            {
                _logger.LogError("User not found in tenant database despite having UserTenant mapping");
                throw new UnauthorizedException("User not found");
            }

            _logger.LogInformation("OTP verified successfully for user in tenant: {TenantId}", userTenant.TenantId);

            var accessToken = await _jwtService.GenerateAccessTokenAsync(user, userTenant.TenantId);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenDurationInDays),
                IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.AccessTokenDurationInMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
} 