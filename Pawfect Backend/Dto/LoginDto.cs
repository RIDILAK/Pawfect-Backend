using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
        public string Password { get; set; }
    }
}

