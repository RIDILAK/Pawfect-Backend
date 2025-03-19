using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _services;
        public CartController(ICartServices services) { 
            _services = services;
        }
        [HttpGet("GetAll")]
        [Authorize(Roles ="User")]
        public async Task <IActionResult> GetAllCarts()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var cart = await _services.GetCartItems(int.Parse(userId));
            return StatusCode(cart.StatusCode, cart);
        }




        [HttpPost("Add")]
        [Authorize(Roles ="User")]
        public async Task <IActionResult> AddToCart(int productId)
        {
           var userId=User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
            var response = await _services.AddToCart(int.Parse(userId), productId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("Remove")]
        [Authorize(Roles ="User")]

        public async Task <IActionResult> RemoveCart(int productId)
        {
            var userId = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
            var res = await _services.RemoveFromCart(int.Parse(userId), productId);
            return StatusCode(res.StatusCode, res);

        }

        [HttpPut("Increase-Quantity")]
        [Authorize]

        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var res = await _services.IncreaseQuantity(int.Parse(userId), productId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("Decrease-Quantity")]
        [Authorize]

        public async Task<IActionResult>DecreaseQuantity(int productId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var res = await _services.DecreaseQuantity(int.Parse(userId), productId);
            return StatusCode(res.StatusCode, res);
        }

    }
}
