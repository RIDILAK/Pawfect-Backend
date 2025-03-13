namespace Pawfect_Backend.Dto
{
    public class CreateOrderDto
    {
        public int AddressId { get; set; }
        public int TotalAmount {  get; set; }
        public string TransactionId {  get; set; }
         
    }
}
