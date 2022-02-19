using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        ILogger<ValuesController> _Logger;

        public ValuesController(ILogger<ValuesController> Logger)
        {
            _Logger = Logger;
        }

        [HttpGet] //GET -> http://localhost:5293/api/values
        public IActionResult Get()
        {
            return Ok(_Values.Values);
        }

        [HttpGet("{Id}")] //GET -> /api/valies/5
        public IActionResult GetById(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            var value = _Values[Id];
            return Ok(value);
        }

        [HttpGet("count")] //GET -> /api/valies/count
        public IActionResult GetCount()
        {
            return Ok(_Values.Count);
        }

        [HttpPost] //POST -> api/values
        [HttpPost("add")] //POST -> api/values/add
        public IActionResult Add([FromBody]string value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;

            _Values[id] = value;

            return Ok(id);
        }

        [HttpPut("{Id}")] //PUT -> api/values/4
        public IActionResult Replace(int Id, [FromBody]string Value)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok();
        }

        [HttpDelete("{Id}")] //DELETE -> api/values/3
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values.Remove(Id);

            return Ok();
        }
    }
}
