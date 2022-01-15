using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Home", "Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
