using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        [Required]
        public int UserId {  get; set; }
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
