﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/employees")] //http://localhost:5001/api/employees
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
            
            //Возвращает 201 статусный код, в параметрах указывается адрес
            //по которому нужно обратиться, чтобы получить только что созданного сотрудника
            return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _EmployeesData.Edit(employee);
            return Ok();
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
