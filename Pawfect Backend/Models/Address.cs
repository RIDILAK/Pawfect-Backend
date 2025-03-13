using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int userId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "House Name is required")]
        [StringLength(100, ErrorMessage = "House Name cannot exceed 100 characters")]
        public string HouseName { get; set; }

        [Required(ErrorMessage = "Place is required")]
        [StringLength(100, ErrorMessage = "Place cannot exceed 100 characters")]
        public string Place { get; set; }

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Pincode must be 6 digits")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        public string State { get; set; }

        public User User { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
