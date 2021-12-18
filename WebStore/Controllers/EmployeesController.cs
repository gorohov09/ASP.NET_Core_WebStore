using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff/{action=Index}/{id?}")]
    public class EmployeesController : Controller
    {
        private static readonly List<Employee> List_Employees = new List<Employee>()
        {
            new Employee() {Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27, Birthday = new DateTime(1996, 12 ,2), Salary = 50000},
            new Employee() {Id = 2, LastName = "Максимов", FirstName = "Максим", Patronymic = "Максимович", Age = 28, Birthday = new DateTime(1997, 4, 1), Salary = 60000},
            new Employee() {Id = 3, LastName = "Сергеев", FirstName = "Сергей", Patronymic = "Сергеевич", Age = 24, Birthday = new DateTime(1994, 8, 4), Salary = 70000},
            new Employee() {Id = 4, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Age = 20, Birthday = new DateTime(1999, 6, 15), Salary = 30000},
            new Employee() {Id = 5, LastName = "Андреев", FirstName = "Андрей", Patronymic = "Андреевич", Age = 21, Birthday = new DateTime(2000, 10, 14), Salary = 45000},
        };

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
    }
}
