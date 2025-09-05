using System.ComponentModel.DataAnnotations;

namespace user_management.DTOs
{
    public class UserUpdateDto
    {   
        public int Id { get; set; } 
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "NRC No is required!")]
        public string NRCNo { get; set; }
    }
}
