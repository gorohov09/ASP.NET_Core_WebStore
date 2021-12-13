using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> List_Employees = new List<Employee>()
        {
            new Employee() {Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Birthday = new DateTime(12, 2, 1996), Salary = 50000},
            new Employee() {Id = 2, LastName = "Максимов", FirstName = "Максим", Patronymic = "Максимович", Birthday = new DateTime(1, 4, 1997), Salary = 60000},
            new Employee() {Id = 3, LastName = "Сергеев", FirstName = "Сергей", Patronymic = "Сергеевич", Birthday = new DateTime(10, 8, 1994), Salary = 70000},
            new Employee() {Id = 4, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Birthday = new DateTime(15, 6, 1999), Salary = 30000},
            new Employee() {Id = 5, LastName = "Андреев", FirstName = "Андрей", Patronymic = "Андреевич", Birthday = new DateTime(14, 10, 2000), Salary = 45000},
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View(List_Employees);
        }
    }
}
