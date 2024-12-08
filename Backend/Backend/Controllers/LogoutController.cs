using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return Ok(new { message = "You have successfully Logged out." });
        }
    }
}
