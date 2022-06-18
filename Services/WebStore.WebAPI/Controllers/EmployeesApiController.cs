using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Employees)] //http://localhost:5001/api/employees
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesApiController(IEmployeesData EmployeesData) { _EmployeesData = EmployeesData; }

        /// <summary>
        /// Получение всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        public IActionResult Get()
        {
            var employees = _EmployeesData.GetAll();
            return Ok(employees);
        }

        /// <summary>
        /// Получение сотрудника по его идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор сотрудника</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int Id)
        {
            var employee = _EmployeesData.GetById(Id);
            return employee == null ? NotFound() : Ok(employee);
        }

        /// <summary>
        /// Добавление нового сотрудника
        /// </summary>
        /// <param name="employee">Новый сотрудник</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Employee))]
        public IActionResult Add(Employee employee)
        {
            var id = _EmployeesData.Add(employee);
            return CreatedAtAction(nameof(GetById), new {Id = id}, employee);
        }

        /// <summary>
        /// Обновление информации о сотруднике
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public IActionResult Update(Employee employee)
        {
            var success = _EmployeesData.Edit(employee);
            return Ok(success);
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="Id">Идентификатор сотрудника</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int Id)
        {
            var result = _EmployeesData.Delete(Id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
