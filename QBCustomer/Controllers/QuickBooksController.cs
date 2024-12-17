using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QBCustomer.Services;
using System.Security.Claims;

namespace QBCustomer.Controllers
{
    [Route("api/QB")]
    [ApiController]
    public class QuickBooksController : ControllerBase
    {
        private readonly QuickBooksService _quickBooksService;
        public QuickBooksController(QuickBooksService quickBooksService)
        {
            _quickBooksService = quickBooksService;
        }
        [Authorize]
        [HttpGet("QBAuthorize")]
        public IActionResult QBAuthorize()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string url = _quickBooksService.InitiateAuth(userId);
            return Ok(url);

        }


        [HttpGet("getToken")]
        public async Task<ActionResult> GetToken()
        {
            string code = Request.Query["code"].ToString() ?? "none";
            string realmId = Request.Query["realmId"].ToString() ?? "none";
            string state = Request.Query["state"].ToString() ?? "none";
            await _quickBooksService.SaveToken(code, realmId,state);
            return Redirect("https://localhost:7170/page/CustomerDetails.html");
        }

      

    }
}
