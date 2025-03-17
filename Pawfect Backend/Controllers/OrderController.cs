using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _services;
        public OrderController(IOrderServices services) { 
         
            _services = services;
        }
        [HttpPost("Place-Order")]
        [Authorize]

        public async Task <IActionResult> Create(CreateOrderDto createOrderDto)
        {
            var userId=User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
            var order= await _services.Create(int.Parse(userId),createOrderDto);
            return StatusCode(order.StatusCode, order);
        }

        [HttpGet("User-Retrival")]
        [Authorize(Roles ="User")]
        public async Task <IActionResult> Get()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var order = await _services.GetOrderDetails(int.Parse(userId));
            return StatusCode(order.StatusCode, order);
        }



        [HttpGet("Admin-Retrival")]
        [Authorize(Roles ="Admin")]
        public async Task <IActionResult> Get(int userId)
        {
            var order=await _services.GetOrderDetails(userId);
            return StatusCode(order.StatusCode, order);
        }
        [HttpGet("All-Orders")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> GetAll()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var response = await _services.GetAllOrders();
            return StatusCode(response.StatusCode, response);

        }
        [HttpGet("Get-Revenue")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> GetRevenue()
        {
            var response = await _services.GetRevenue();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("Update_status")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> ChangeOrderStatus(int OrderId, AddStatusDto addStatusDto)
        {
            var response = await _services.ChangeStatus(OrderId, addStatusDto.OrderStatus);
            return StatusCode(response.StatusCode, response);
        }


    }
}
