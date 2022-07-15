using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValueService _ValuesService;

        private readonly IPersonsService _PersonsService;

        public WebAPIController(IValueService ValueService, IPersonsService PersonsService)
        {
            _ValuesService = ValueService;
            _PersonsService = PersonsService;
        }

        public async Task<IActionResult> Index()
        {
            var values = _ValuesService.GetValues();
            //var persons = await _PersonsService.GetPersons();

            return View(values);
        }
    }
}
