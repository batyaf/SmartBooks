using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QBCustomer.Models;
using QBCustomer.Services;
using QBCustomer.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QBCustomer.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                 await _authService.RegisterUser(request);
                 return Ok(new { Message = "Registration successful. Please login." });
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
          

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
           var token= await _authService.LoginUser(request);
            return Ok(new { Token=token });
        }

     

    }
}
