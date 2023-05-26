using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TspuWebLabs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery] int count)
        {
            Random random = new Random();
            IEnumerable<int> values = Enumerable.Range(1, count).Select(x => random.Next(Int32.MinValue, Int32.MaxValue));

            return Ok(values);
        }
    }
}