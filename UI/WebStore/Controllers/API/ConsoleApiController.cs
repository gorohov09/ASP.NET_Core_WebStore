using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers.API
{
    [Route("api/console")]
    [ApiController]
    public class ConsoleApiController : ControllerBase
    {
        [HttpGet("clear")]
        public void Clear() => Console.Clear();

        [HttpGet("write")]
        public void WriteLine(string message) => Console.WriteLine(message);
    }
}
