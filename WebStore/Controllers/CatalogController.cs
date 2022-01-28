﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IActionResult Index(int? BrandId, int? SectionId)
        {
            var filter = new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId
            };

            var products = _ProductData.GetProducts(filter);

            var catalog_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products
                .OrderBy(p => p.Order)
                .ToView()
            };

            return View(catalog_model);
        }

        public IActionResult Details(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            var product_viewmodel = product.ToView();

            return View(product_viewmodel);
        }
    }
}
