namespace StarterApi.Application.Modules.Societies.DTOs
{
    public class SocietyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int TotalBlocks { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class CreateSocietyDto
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class UpdateSocietyDto
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }
}
