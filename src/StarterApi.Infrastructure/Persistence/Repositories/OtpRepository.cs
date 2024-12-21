using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using StarterApi.Domain.Entities;
using StarterApi.Infrastructure.Persistence.Contexts;

namespace StarterApi.Infrastructure.Persistence.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly RootDbContext _context;
        private readonly ILogger<OtpRepository> _logger;

        public OtpRepository(RootDbContext context, ILogger<OtpRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OtpRequest> CreateAsync(OtpRequest otpRequest)
        {
            // Invalidate any existing OTPs for this mobile number
            var existingOtps = await _context.OtpRequests
                .Where(o => o.MobileNumber == otpRequest.MobileNumber)
                .ToListAsync();

            foreach (var otp in existingOtps)
            {
                otp.IsVerified = true; // Mark as used
                otp.ExpiresAt = DateTime.UtcNow; // Expire it
            }

            await _context.OtpRequests.AddAsync(otpRequest);
            return otpRequest;
        }

        public async Task<OtpRequest> GetLatestOtpRequestAsync(string mobileNumber)
        {
            return await _context.OtpRequests
                .Where(o => o.MobileNumber == mobileNumber)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateOtpAsync(string mobileNumber, string otpCode)
        {
            _logger.LogInformation("Validating OTP for mobile: {MobileNumber}", mobileNumber);

            var latestOtp = await GetLatestOtpRequestAsync(mobileNumber);
            _logger.LogInformation("Latest OTP request found: {Found}, IsVerified: {IsVerified}, Expired: {IsExpired}", 
                latestOtp != null,
                latestOtp?.IsVerified,
                latestOtp?.ExpiresAt < DateTime.UtcNow);

            // Check if OTP exists and is not expired
            if (latestOtp == null || latestOtp.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid OTP state for {MobileNumber}", mobileNumber);
                return false;
            }

            // Check if OTP matches, regardless of verification status
            var isValid = latestOtp.OtpCode == otpCode;
            _logger.LogInformation("OTP code match result: {IsValid}", isValid);

            if (isValid)
            {
                latestOtp.IsVerified = true;
                await SaveChangesAsync();
                _logger.LogInformation("OTP marked as verified for {MobileNumber}", mobileNumber);
            }

            return isValid;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
} 