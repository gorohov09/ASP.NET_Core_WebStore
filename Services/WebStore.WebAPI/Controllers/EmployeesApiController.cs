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
        private readonly ILogger<EmployeesApiController> _Logger;

        public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger) 
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        /// <summary>
        /// Получение всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        public IActionResult Get()
        {
            _Logger.LogInformation("Начало процесса получения сотрудников");
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
            _Logger.LogInformation("Получение сотрудника по Id");
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
            _Logger.LogInformation("Добавление сотрудника");
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
            _Logger.LogInformation("Обновление информации о сотруднике");
            var success = _EmployeesData.Edit(employee);
            if (success)
                _Logger.LogInformation("Информация о сотруднике была успешно обновлена");
            else
                _Logger.LogWarning("Произошла ошибка при обновлении информации о сотруднике");
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
            _Logger.LogInformation("Удаление сотрудника по Id");
            var result = _EmployeesData.Delete(Id);
            if (result)
                _Logger.LogInformation("Сотрудник успешно удален");
            else
                _Logger.LogWarning("Произошла ошибка при удалении сотрудника");
            return result ? Ok(true) : NotFound(false);
        }
    }
}
