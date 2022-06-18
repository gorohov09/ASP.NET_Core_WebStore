using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using WebStore.ViewModels;

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

        //public void Throw(string Message) => throw new ApplicationException(Message);
    }
}
