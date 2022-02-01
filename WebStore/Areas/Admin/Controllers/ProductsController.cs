using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Areas.Admin.ViewModels;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<ProductsController> _Logger;

        public ProductsController(IProductData ProductData, ILogger<ProductsController> Logger)
        {
            _ProductData = ProductData;
            _Logger = Logger;
        }

        public IActionResult Index()
        {
            var products = _ProductData.GetProducts();

            return View(products);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var product = _ProductData.GetProductById(Id); //Получаем товар по идентификатору

            if (product is null) //Смотрим, что в каталоге такой товар есть
                return NotFound(); //Если товара нет - 404 ошибка

            //Если есть - формируем ViewModel, в которую заполняем все данные по товару
            return View(new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                SectionId = product.SectionId,
                Section = product.Section.Name,
                BrandId = product.BrandId,
                Brand = product.Brand.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            });
        }

        [HttpPost]
        public IActionResult Edit(EditProductViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var product = _ProductData.GetProductById(Model.Id);

            if (product is null)
                NotFound();

            //Копируем данные из модели в объект товара
            //product.Name = Model.Name;
            //product.Order = Model.Order;
            //product.Price = Model.Price;
            //product.ImageUrl = Model.ImageUrl;

            //var brand = _ProductData.GetBrandById(Model.BrandId ?? -1);
            //var section = _ProductData.GetSectionById(Model.SectionId);

            //product.Brand = brand;
            //product.Section = section;

            //// отредактировать товар используя сервис productData
            //_ProductData.Update(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] //Отправляет форму с подтверждением, что нужно удалить товар
        public IActionResult Delete(int Id)
        {
            var product = _ProductData.GetProductById(Id); //Получаем товар по идентификатору

            if (product is null) //Смотрим, что в каталоге такой товар есть
                return NotFound(); //Если товара нет - 404 ошибка

            //Если есть - формируем ViewModel, в которую заполняем все данные по товару
            return View(new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                SectionId = product.SectionId,
                Section = product.Section.Name,
                BrandId = product.BrandId,
                Brand = product.Brand.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            });
        }

        [HttpPost] //Выполняет удаление
        public IActionResult DeleteConfirmed(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            //Удалить product, используя сервис _ProductData
            //_ProductData.Delete(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
