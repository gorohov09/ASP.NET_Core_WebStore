using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Employees()
        //{
        //    return View(List_Employees);
        //}

        //public IActionResult Employee(int id)
        //{
        //    if ((id < 1) || (id > List_Employees.Count))
        //    {
        //        return BadRequest(404);
        //    }

        //    var employee = List_Employees.First(e => e.Id == id);
        //    return View(employee);
        //}
    }
}
