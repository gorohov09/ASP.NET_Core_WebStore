using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Employees;
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

            return Ok(employees.ToDTO());
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            var employee = _EmployeesData.GetById(Id);

            if (employee is null)
                return NotFound();

            return Ok(employee.ToDTO());
        }

        [HttpPost]
        public IActionResult Add(EmployeeDTO employee)
        {
            var id = _EmployeesData.Add(employee.FromDTO());

            return CreatedAtAction(nameof(GetById), new { id }, employee);
        }

        [HttpPut]
        public IActionResult Edit(EmployeeDTO employee)
        {
            var success = _EmployeesData.Edit(employee.FromDTO());
            return Ok(success);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var result = _EmployeesData.Delete(Id);

            return result 
                ? Ok(true)
                : NotFound(false);
        }
    }
}
