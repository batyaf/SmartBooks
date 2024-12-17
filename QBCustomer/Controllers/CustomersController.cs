using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        public async Task<IActionResult> getCoustomeFromQB()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var output = await _customersService.getCustomerFromQBApi(userId);
            return Ok(output);
        }



        [HttpGet]
        public  async Task<IActionResult> getCoustomerfromDb()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var output =await _customersService.GetCustomer(userId);
            return Ok(output);
        }
    }
}


