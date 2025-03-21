using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        public int userId {  get; set; }
        public int AddressId {  get; set; }
        public int TotalPrice { get; set; }
        public string OrderStatus { get; set; } 
        public DateTime OrderTime { get; set; }
        public string TransactionId {  get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
    }

    

}
