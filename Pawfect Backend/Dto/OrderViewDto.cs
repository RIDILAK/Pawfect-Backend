namespace Pawfect_Backend.Dto
{
    public class OrderViewDto
    {
        public int ProductId {  get; set; }
        public int orderItemId {  get; set; }
        public string ProductName {  get; set; }
        public string Url { get; set; }
        public int price { get; set; }
        public decimal TotalAmount {  get; set; }
        public int Quantity {  get; set; }


    }
}
