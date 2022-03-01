using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _EmployeesData.GetAll();

            return Ok(employees);
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            var employee = _EmployeesData.GetById(Id);

            if (employee is null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var id = _EmployeesData.Add(employee);

            return CreatedAtAction(nameof(GetById), new { id }, employee);
        }

        [HttpPut]
        public IActionResult Edit(Employee employee)
        {
            _EmployeesData.Edit(employee);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _EmployeesData.Delete(id);

            return result 
                ? Ok(true)
                : NotFound(false);
        }
    }
}
