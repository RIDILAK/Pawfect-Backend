namespace Pawfect_Backend.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId {  get; set; }
        public int ProductId {  get; set; }
        public int Quantity {  get; set; }
        public int TotalPrice {  get; set; }

        public Products Product { get; set; }
        public Orders Orders { get; set; }
    }
}
