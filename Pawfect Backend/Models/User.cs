using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public string Role { get; set; }

        public bool isBlocked { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public List<WishList> wishList { get; set; }

        public Cart Cart { get; set; }
    }
}
