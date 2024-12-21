public class OtpRequest : BaseEntity
{
    public string MobileNumber { get; set; }
    public string OtpCode { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsVerified { get; set; }
    public Guid? UserId { get; set; }
    public User User { get; set; }
} 