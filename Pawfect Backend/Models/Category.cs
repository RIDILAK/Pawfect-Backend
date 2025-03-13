using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Category Name is required")]
        [MaxLength(50,ErrorMessage ="CategoryName Cannot be more than 50 characters")]
        public string CategoryName { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
