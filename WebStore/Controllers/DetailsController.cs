using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
