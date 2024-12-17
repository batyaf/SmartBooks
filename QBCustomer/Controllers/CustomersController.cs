using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QBCustomer.Services;
using System.Security.Claims;

namespace QBCustomer.Controllers
{
    [Authorize]
    [Route("api/Coustomer")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomersService _customersService;
        
        public CustomersController(CustomersService customersService) {
           _customersService = customersService;
        }


        [HttpGet("getFromQB")]
        public async Task<IActionResult> GetCoustomeFromQB()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var output = await _customersService.GetCustomerFromQBApi(userId);
                return Ok(output);
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("getCustomer")]
        public  async Task<IActionResult> GetCoustomerfromDb()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var output = await _customersService.GetCustomer(userId);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}


