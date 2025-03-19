using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
       private readonly IWishListServices _wishListServices;

        public WishListController(IWishListServices wishListServices)
        {
            _wishListServices = wishListServices;
        }
        [HttpPost("Add-or-Remove")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> AddOrRemoveWishList(int ProductId) 
        {

            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responses = await _wishListServices.AddOrRemoveWishList(ProductId,int.Parse(userId));
            return StatusCode(responses.StatusCode, responses);
        }

        [HttpGet("Get-All")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> GetWishList()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var response = await _wishListServices.GetWishList(int.Parse(userId));
            return StatusCode(response.StatusCode, response);
        }


    }
}
