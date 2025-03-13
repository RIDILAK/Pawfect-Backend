using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usercontroller : ControllerBase
    {
        private readonly IUserServices _userServices;

        public Usercontroller(IUserServices userServices) { 
             
            _userServices = userServices;
        
        }

        [HttpPost("SignUp")]
        public async Task <IActionResult> SignUp([FromBody]SignUpDto AddUser) { 
        
            var result=await _userServices.SignUpUsers(AddUser);
            return StatusCode(result.StatusCode,result);
        
        }
       
        [HttpPost("loginUser")]

        public async Task <IActionResult> LoginUser(LoginDto login)
        {
            var user =await _userServices.LoginUsers(login);
            return StatusCode(user.StatusCode, user);
          
        }
    }
}
