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

public class UserTenantInfoDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; }
    public string Role { get; set; }
}

public class AuthResponseDto
{
    public string BaseToken { get; set; }
    public string RefreshToken { get; set; }
    public UserDto User { get; set; }
    public List<UserTenantInfoDto> AvailableTenants { get; set; }
}

public class SetTenantRequestDto
{
    public string BaseToken { get; set; }
    public Guid TenantId { get; set; }
}

public class TenantContextResponseDto
{
    public string AccessToken { get; set; }
    public TenantContextDto TenantContext { get; set; }
}

public class TenantContextDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; }
    public string Role { get; set; }
    public List<string> Permissions { get; set; }
} 