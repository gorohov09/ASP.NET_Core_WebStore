using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _Logger = Logger;
            _EmployeesData = EmployeesData;
        }

        public IActionResult Index()
        {
            var result = _EmployeesData.GetAll();
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.GetById(id);

            if (employee is null)
                return NotFound();
            
            return View(employee);
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View(new EmployeeViewModel());
            }
            else
            {
                var employee = _EmployeesData.GetById((int)id);

                if (employee is null)
                {
                    _Logger.LogWarning("При редактировании сотрудника с id:{0} он не был найден", id);
                    return NotFound();
                }
                    
                var model = new EmployeeViewModel()
                {
                    Id = employee.Id,
                    LastName = employee.LastName,
                    FirstName = employee.FirstName,
                    Patronymic = employee.Patronymic,
                    Age = employee.Age,
                    Birthday = employee.Birthday,
                    Salary = employee.Salary
                };

                return View(model);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (Model.LastName == "Асама" && Model.FirstName == "Бин" && Model.Patronymic == "Ладен")
                ModelState.AddModelError("", "Террористов на работу не берем!");

            if (!ModelState.IsValid)
                return View(Model);

            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Patronymic = Model.Patronymic,
                Age = Model.Age,
                Birthday = Model.Birthday,
                Salary = Model.Salary
            };

            if (Model.Id == 0)
            {
                _EmployeesData.Add(employee);
                _Logger.LogInformation("Создание нового сотрудника {0}", employee);

            }

            else if (!_EmployeesData.Edit(employee))
                return NotFound();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            
            var employee  =_EmployeesData.GetById(id);

            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel()
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_EmployeesData.Delete(id))
                return NotFound();
            _Logger.LogInformation("Сотрудник с id:{0} удален", id);

            return RedirectToAction("Index");
        }
    }
}
