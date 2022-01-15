using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterUserViewModel registerUserView = new RegisterUserViewModel();

            return View(registerUserView);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User()
            {
                UserName = model.UserName,
            };

            var registration_result = await _userManager.CreateAsync(user);

            if (registration_result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                RedirectToAction("Home", "Index");
            }

            foreach (var error in registration_result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Login()
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
