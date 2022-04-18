using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private static readonly List<Person> _People = new List<Person>
        {
            new Person { Id = 1, LastName = "Горохов", FirstName = "Андрей", Age = 19, Birthday = new DateTime(2002, 7, 9), Salary = 100000},
            new Person { Id = 2, LastName = "Курочкин", FirstName = "Владислав", Age = 18, Birthday = new DateTime(2002, 6, 19), Salary = 90000},
            new Person { Id = 3, LastName = "Ардинцев", FirstName = "Максим", Age = 30, Birthday = new DateTime(1996, 12, 7), Salary = 20000}
        };

        private readonly ILogger<PersonController> _Logger;

        public PersonController(ILogger<PersonController> _logger) { _Logger = _logger; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_People);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var person = _People.FirstOrDefault(person => person.Id == Id);

            if (person is null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            return Ok(_People.Count);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Person person)
        {
            if (person is null)
                return NotFound();

            _People.Add(person);

            return CreatedAtAction(nameof(GetById), new { person.Id}, person);
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> Delete(int Id)
        {
            var person = _People.FirstOrDefault(person => person.Id == Id);

            if (person is null)
                return NotFound();

            _People.Remove(person);

            return Ok();
        }
    }
}
