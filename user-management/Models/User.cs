using System.ComponentModel.DataAnnotations;

namespace user_management.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required!")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        
        [RegularExpression(@"^09\d{7,9}$", ErrorMessage = "Invalid Myanmar phone number.")]

        public string ContactNo { get; set; } 
        [Required(ErrorMessage = "NRC No is required!")]
        public string NRCNo { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; }
    }
}
