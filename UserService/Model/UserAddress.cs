using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserService.Model
{
    public class UserAddress
    {
        [Key]
        public Guid Id { get; set; }  // Primary Key

        [ForeignKey("User")]
        public string UserId { get; set; }  // Foreign Key
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary { get; set; }  // Primary address flag
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
