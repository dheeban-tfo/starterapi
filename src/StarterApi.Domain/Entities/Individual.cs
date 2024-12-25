namespace StarterApi.Domain.Entities
{
    public class Individual : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? AlternatePhoneNumber { get; set; }
        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public DateTime? LastVerifiedAt { get; set; }
        public bool IsVerified { get; set; }
        public Guid? VerifiedBy { get; set; }

        // Navigation properties
        private readonly List<Owner> _owners = new();
        public IReadOnlyCollection<Owner> Owners => _owners.AsReadOnly();
    }
}
