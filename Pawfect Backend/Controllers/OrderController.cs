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
        private readonly IRazorpayOrderService _orderService;
       
        public OrderController(IOrderServices services,IRazorpayOrderService razorpayOrderService) { 
         
            _services = services;
            _orderService = razorpayOrderService;
        }
        [HttpPost("Create-RazorPay")]
        [Authorize]
        public async Task<IActionResult> CreateRazorPayOrder( int price)
        {
           var result=await _order Service.CreateRazorpayOrder(price);
            return StatusCode(result.StatusCode,result);
        }

        [HttpPost("Verify-Payment")]
        [Authorize]

        public async Task<IActionResult>VerifyPayment(PaymentDto payment)
        {
            var result = await _orderService.RazorPayment(payment);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Place")]
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
        [HttpGet("Get-All")]
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

        [HttpPatch("Update-status")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> ChangeOrderStatus(int OrderId, AddStatusDto addStatusDto)
        {
            var response = await _services.ChangeStatus(OrderId, addStatusDto.OrderStatus);
            return StatusCode(response.StatusCode, response);
        }


    }
}
