using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string url = _quickBooksService.InitiateAuth(userId);
                return Ok(url);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("getToken")]
        public async Task<ActionResult> GetToken()
        {
            string code = Request.Query["code"].ToString();
            string realmId = Request.Query["realmId"].ToString();
            string state = Request.Query["state"].ToString();
            if (code != null && realmId != null && state != null)
            {
                await _quickBooksService.SaveToken(code, realmId, state);
            }
            return Redirect("https://localhost:7170/page/CustomerDetails.html");
        }
    }
}
