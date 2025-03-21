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
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressServices;
        public AddressController(IAddressService addressServices) { 
        
            _addressServices = addressServices;
        }

        [HttpPost("Add")]
        [Authorize (Roles =("User"))]

        public async Task<IActionResult> AddAddress(AddressCreateDto newAddress)
        {
           var userId=User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
            var responses= await _addressServices.AddAddress(int.Parse(userId),newAddress);
            return StatusCode(responses.StatusCode, responses);
        }

        [HttpGet("Get-All")]
        [Authorize]

        public async Task <IActionResult> Get()
        {
            var userid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responses = await _addressServices.GetAddresses(int.Parse(userid));
            return StatusCode(responses.StatusCode, responses);
        }

        [HttpDelete("Remove")]
        [Authorize(Roles ="User")]
          
        public async Task <IActionResult> Remove(int AddressId)
        {
            var userid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var responses = await _addressServices.RemoveAddress(int.Parse(userid), AddressId);
            return StatusCode(responses.StatusCode,responses);
        }
      
    }
}
