using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private ICollection<Employee> List_Employees;

        public EmployeesController()
        {
            List_Employees = TestData.Employees;
        }

        public IActionResult Index()
        {
            var result = List_Employees;
            return View(result);
        }

        public IActionResult Details(int id)
        {
            if ((id < 1) || (id > List_Employees.Count))
            {
                return BadRequest(404);
            }

            var employee = List_Employees.First(e => e.Id == id);
            return View(employee);
        }

        public IActionResult Create() => View();

        public IActionResult Edit(int id)
        {
            var employee = List_Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            var model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            };

            return View(model);
        }

        public IActionResult Edit(EmployeeEditViewModel Model)
        {
            // Обработка модели

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) => View();
    }
}
