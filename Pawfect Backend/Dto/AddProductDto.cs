using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Dto
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
