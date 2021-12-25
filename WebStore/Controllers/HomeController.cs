using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
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
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price
                });

            ViewBag.Products = products;

            return View();
        }

        //public void Throw(string Message) => throw new ApplicationException(Message);
    }
}
