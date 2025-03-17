using Pawfect_Backend.Models;

namespace Pawfect_Backend.Dto
{
    public class ViewUserOrderDetailsDto
    {

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderId { get; set; }
        public int TotalPrice { get; set; }
        public string OrderStatus { get; set; } 
        public AddressCreateDto Address {  get; set; }

        public string TransactionId {  get; set; }

        public List<OrderViewDto> OrderProduct { get; set; }
    }
}
