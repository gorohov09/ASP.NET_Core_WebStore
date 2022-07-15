using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IProductData _ProductData)
        {
            var products = _ProductData.GetProducts()
                .OrderBy(p => p.Order)
                .Take(6)
                .ToView();

            ViewBag.Products = products;

            return View();
        }

        public string ConfiguredAction(string id, string Value1)
        {
            return $"Hello World! {id} - {Value1}";
        }

        public void Throw(string Message) => throw new ApplicationException(Message);

        public IActionResult Status(string Code)
        {
            return Content($"Status code - {Code}");
        }
    }
}
