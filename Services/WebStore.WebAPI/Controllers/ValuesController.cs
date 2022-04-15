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

        private readonly ILogger<ValuesController> _Logger;

        public ValuesController(ILogger<ValuesController> _logger) { _Logger = _logger; }

        [HttpGet] //GET -> http://localhost:5001/api/values
        public IActionResult Get()
        {
            return Ok(_Values.Values);
        }

        [HttpGet("{Id}")] //GET -> http://localhost:5001/api/values/5
        public IActionResult GetById(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            return Ok(_Values[Id]);
        }

        [HttpGet("count")] //GET -> http://localhost:5001/api/values/count
        public IActionResult Count()
        {
            return Ok(_Values.Count);
        }

        [HttpPost]
        [HttpPost("add")] //POST -> api/values/add
        public IActionResult Add([FromBody]string Value)
        {
            if (Value is null)
                return NotFound();

            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            
            _Values.Add(id, Value);

            return CreatedAtAction(nameof(GetById), new { id }, Value);
        }

        [HttpPut("{Id}")] //PUT -> api/values/5
        public IActionResult Replace(int Id, [FromBody]string Value)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok();
        }

        [HttpDelete("{Id}")] //DELETE -> api/values/5
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values.Remove(Id);

            return Ok();
        }
    }
}
