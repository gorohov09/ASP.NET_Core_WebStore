using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValueService _ValuesService;

        public WebAPIController(IValueService ValueService)
        {
            _ValuesService = ValueService;
        }

        public IActionResult Index()
        {
            var values = _ValuesService.GetValues();

            return View(values);
        }
    }
}
