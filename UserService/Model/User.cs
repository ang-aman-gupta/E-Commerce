using System.ComponentModel.DataAnnotations;

namespace UserService.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }  // Primary Key

        public string IdentityUserId { get; set; }  // FK from AuthService
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
