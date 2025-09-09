using System.ComponentModel.DataAnnotations;

namespace user_management.ViewModels
{
    public class ForgotPasswordViewModel

    {
        [Required(ErrorMessage ="Email is required!" )]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "New Password is required!")]

        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required!")]

        public string ConfirmPassword { get; set; }
    }
}
