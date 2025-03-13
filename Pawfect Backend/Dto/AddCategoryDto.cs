using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Dto
{
    public class AddCategoryDto
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
        public string CategoryName { get; set; }
    }
}
