namespace AuthService.DTOs
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
