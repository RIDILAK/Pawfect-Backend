namespace Pawfect_Backend.Dto
{
    public class GetProductsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public string Url { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Rating { get; set; }

        public string Description { get; set; }
        public string CategoryName { get; set; }
    }
}
