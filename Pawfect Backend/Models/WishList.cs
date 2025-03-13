using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class WishList
    {
        [Required]
        public int WishListId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]

        public int  ProductId {get;set;}
        public Product Product { get; set; }
    }
}
