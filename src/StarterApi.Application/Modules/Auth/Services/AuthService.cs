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
using System.Security.Claims;
using System.Collections.Generic;

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
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantTokenService _tenantTokenService;

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
            IRefreshTokenRepository refreshTokenRepository,
            ITenantRepository tenantRepository,
            ITenantTokenService tenantTokenService)
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
            _tenantRepository = tenantRepository;
            _tenantTokenService = tenantTokenService;
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

            var baseToken = _jwtService.GenerateBaseToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Get user's tenants
            var userTenants = await _userTenantRepository.GetByUserIdAsync(user.Id);
            var availableTenants = userTenants.Select(ut => new UserTenantInfoDto
            {
                TenantId = ut.TenantId,
                TenantName = ut.Tenant.Name,
                Role = ut.RoleId.ToString()
            }).ToList();

            return new AuthResponseDto
            {
                BaseToken = baseToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user),
                AvailableTenants = availableTenants
            };
        }

        public async Task<TenantContextResponseDto> SetTenantAsync(SetTenantRequestDto request)
        {
            _logger.LogInformation("Setting tenant context. TenantId: {TenantId}", request.TenantId);

            try
            {
                var principal = _jwtService.ValidateToken(request.BaseToken);
                var tokenType = principal.FindFirst("token_type")?.Value;
                
                _logger.LogInformation("Token type: {TokenType}", tokenType);

                if (tokenType != "base_token")
                {
                    _logger.LogWarning("Invalid token type: {TokenType}", tokenType);
                    throw new UnauthorizedException("Invalid token type - expected 'base_token'");
                }

                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                    ?? throw new UnauthorizedException("User ID not found in token"));

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException("User not found");

                var tenantToken = await _tenantTokenService.GenerateTenantTokenAsync(user, request.TenantId);
                var userTenant = await _userTenantRepository.GetByUserAndTenantIdAsync(userId, request.TenantId);
                var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);

                return new TenantContextResponseDto
                {
                    AccessToken = tenantToken,
                    TenantContext = new TenantContextDto
                    {
                        TenantId = tenant.Id,
                        TenantName = tenant.Name,
                        Role = userTenant.RoleId.ToString(),
                        Permissions = new List<string>() // TODO: Get actual permissions
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting tenant context");
                throw;
            }
        }
    }
} 