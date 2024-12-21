public interface IAuthService
{
    Task<bool> RequestOtpAsync(OtpRequestDto request);
    Task<AuthResponseDto> VerifyOtpAsync(OtpVerificationDto request);
} 