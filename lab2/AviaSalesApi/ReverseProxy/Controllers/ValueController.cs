using Microsoft.AspNetCore.Mvc;

namespace ReverseProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(12);
        }
    }
}