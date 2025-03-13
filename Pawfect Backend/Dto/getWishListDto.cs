namespace Pawfect_Backend.Dto
{
    public class getWishListDto
    {
        public int WishListId { get; set; }
        public string ProductName { get; set; }

        public string Url { get; set; }
        public int Price { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }

    }
}
