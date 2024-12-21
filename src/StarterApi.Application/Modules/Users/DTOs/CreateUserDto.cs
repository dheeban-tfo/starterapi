namespace StarterApi.Application.Modules.Users.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType UserType { get; set; }
    }
} 