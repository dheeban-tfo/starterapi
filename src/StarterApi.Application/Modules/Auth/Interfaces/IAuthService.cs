public interface IAuthService
{
    Task<bool> RequestOtpAsync(OtpRequestDto request);
    Task<AuthResponseDto> VerifyOtpAsync(OtpVerificationDto request);
    Task<TenantContextResponseDto> SetTenantAsync(SetTenantRequestDto request);
} 