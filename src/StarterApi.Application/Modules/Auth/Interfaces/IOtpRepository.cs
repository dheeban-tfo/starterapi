public interface IOtpRepository
{
    Task<OtpRequest> CreateAsync(OtpRequest otpRequest);
    Task<OtpRequest> GetLatestOtpRequestAsync(string mobileNumber);
    Task<bool> ValidateOtpAsync(string mobileNumber, string otpCode);
    Task SaveChangesAsync();
} 