using Pawfect_Backend.Models;

namespace Pawfect_Backend.Dto
{
    public class AddRemoveWishListDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
