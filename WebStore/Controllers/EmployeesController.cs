using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData)
        {
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

        public IActionResult Create() => View();

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _EmployeesData.GetById(id);

            if (employee == null)
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
        public IActionResult Edit(EmployeeViewModel Model)
        {
            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Patronymic = Model.Patronymic,
                Age = Model.Age

            };

            if (!_EmployeesData.Edit(employee))
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
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_EmployeesData.Delete(id))
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
