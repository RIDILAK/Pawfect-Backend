namespace Pawfect_Backend.Dto
{
    public class CartViewDto
    {
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public List<CartItemViewDto> Items { get; set; }
    }
}
