using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")] //Указываем, что данный контроллер находится в области администратора
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
