public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
} 