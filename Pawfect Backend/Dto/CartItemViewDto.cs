namespace Pawfect_Backend.Dto
{
    public class CartItemViewDto
    {
       public int ProductId {  get; set; }
        public string ProductName { get; set; }
        public string Url { get; set; }

        public int Quantity {  get; set; }
        public decimal Price { get; set; }

      

   


    }
}
