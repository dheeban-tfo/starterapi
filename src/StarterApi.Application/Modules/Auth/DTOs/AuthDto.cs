using StarterApi.Application.Modules.Users.DTOs;

public class OtpRequestDto
{
    public string MobileNumber { get; set; }
}

public class OtpVerificationDto
{
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
}

public class AuthResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; }
} 