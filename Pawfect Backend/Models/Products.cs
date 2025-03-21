using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pawfect_Backend.Models
{
    public class Products
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Product Name is Required")]
        [MaxLength(100,ErrorMessage ="Product Name can't be more than 100 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage ="Price is Required")]
        [Range(1,int.MaxValue,ErrorMessage ="Price is must be greater than 0")]
        public int Price { get; set; }

        [Required(ErrorMessage ="Image Url is Required")]
        [Url(ErrorMessage ="Invalid Url Message")]
        public string Url {  get; set; }

        [Range(0,5,ErrorMessage ="Rating must be between 0 and 5")]
        public int Rating {  get; set; }

        [Required(ErrorMessage ="Description is required")]
        [MaxLength(500,ErrorMessage ="Description can't be more than 500 charcters")]
        public string Description {  get; set; }
        [Required]
        public int Quantity {  get; set; }

        [Required(ErrorMessage ="Category id is required")]
          public int CategoryId { get; set; }
        public Category category { get; set; }

        public List<WishList> wishList { get; set; }
    }
}
