using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff/{action=Index}/{id?}")]
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
    }
}
