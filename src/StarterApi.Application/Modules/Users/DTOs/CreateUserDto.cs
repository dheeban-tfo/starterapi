namespace StarterApi.Application.Modules.Users.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public UserType UserType { get; set; }
    }

    // public class UserDto
    // {
    //     public Guid Id { get; set; }
    //     public string Name { get; set; }
    //     public string Email { get; set; }
    //     public string MobileNumber { get; set; }
    //     public DateTime CreatedAt { get; set; }
    // }
} 