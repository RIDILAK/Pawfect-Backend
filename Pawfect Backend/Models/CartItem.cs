using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId {  get; set; }
        [Required]
        public int Quantity {  get; set; }

        public Product Product { get; set; }
        public Cart Cart { get; set; }


    }
}
