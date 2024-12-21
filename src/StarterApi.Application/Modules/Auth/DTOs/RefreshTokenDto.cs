public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; }
    public Guid TenantId { get; set; }
}

public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
} 