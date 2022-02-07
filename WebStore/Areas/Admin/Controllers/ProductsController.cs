using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Areas.Admin.ViewModels;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        private readonly ILogger<ProductsController> _Logger;

        public ProductsController(IProductData productData, ILogger<ProductsController> logger)
        {
            _ProductData = productData;
            _Logger = logger;
        }

        public IActionResult Index()
        {
            var products = _ProductData.GetProducts().ToView();

            return View(products);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            var model = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Section = product.Section.Name,
                SectionId = product.SectionId,
                BrandId = product.BrandId,
                Brand = product.Brand.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var product = _ProductData.GetProductById(Model.Id);

            if (product is null)
                return NotFound();

            //Копируем данные
            product.Name = Model.Name;
            product.ImageUrl = Model.ImageUrl;
            product.Price = Model.Price;
            product.Order = Model.Order;

            var brand = _ProductData.GetBrandById(Model.BrandId ?? -1);
            var section = _ProductData.GetSectionById(Model.SectionId);

            product.Brand = brand;
            product.Section = section;

            _ProductData.Edit(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            var model = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Section = product.Section.Name,
                SectionId = product.SectionId,
                BrandId = product.BrandId,
                Brand = product.Brand.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            _ProductData.Delete(product.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
