using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Individuals.DTOs
{
    public class IndividualDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhoneNumber { get; set; }
        public string IdProofType { get; set; }
        public string IdProofNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public DateTime? LastVerifiedAt { get; set; }
        public bool IsVerified { get; set; }
        public Guid? VerifiedBy { get; set; }
    }

    public class CreateIndividualDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Phone]
        public string? AlternatePhoneNumber { get; set; }

        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(10)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(50)]
        public string EmergencyContactName { get; set; }

        [Required]
        [Phone]
        public string EmergencyContactNumber { get; set; }
    }

    public class UpdateIndividualDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        public string AlternatePhoneNumber { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(10)]
        public string Pincode { get; set; }

        [StringLength(100)]
        public string EmergencyContactName { get; set; }

        [StringLength(20)]
        public string EmergencyContactNumber { get; set; }
    }

    public class VerifyIndividualDto
    {
        [Required]
        public bool IsVerified { get; set; }

        [Required]
        [StringLength(500)]
        public string VerificationNotes { get; set; }
    }
}
