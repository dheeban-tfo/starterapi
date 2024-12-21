using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StarterApi.Application.Common.Exceptions;
using StarterApi.Application.Modules.Users.DTOs;


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

        public AuthService(
            IOtpRepository otpRepository,
            IUserTenantRepository userTenantRepository,
            ITenantProvider tenantProvider,
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _otpRepository = otpRepository;
            _userTenantRepository = userTenantRepository;
            _tenantProvider = tenantProvider;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
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

            return new AuthResponseDto
            {
                Token = "mock_jwt_token", // TODO: Implement JWT generation
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
} 