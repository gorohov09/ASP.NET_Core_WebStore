using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        [HttpGet("sections")] //GET -> http://localhost:5001/api/products/sections
        public IActionResult GetSections()
        {
            var sections = _ProductData.GetSections();
            return Ok(sections);
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _ProductData.GetBrands();
            return Ok(brands);
        }

        [HttpGet("sections/{Id}")]
        public IActionResult GetSectionById(int Id)
        {
            var section = _ProductData.GetSectionById(Id);
            if (section is null)
                return NotFound();
            return Ok(section);
        }

        [HttpGet("brands/{Id}")]
        public IActionResult GetBrandsById(int Id)
        {
            var brand = _ProductData.GetBrandById(Id);
            if (brand is null)
                return NotFound();
            return Ok(brand);
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter? Filter = null)
        {
            var products = _ProductData.GetProducts(Filter);
            return Ok(products);
        }

        [HttpGet("{Id}")]
        public IActionResult GetProductById(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("new/{Name}")]
        public IActionResult CreateProduct(CreateProductDTO productDTO)
        {
            var product = _ProductData.CreateProduct(productDTO.Name, productDTO.Order, productDTO.Price, 
                productDTO.ImageUrl, productDTO.Section, productDTO.Brand);

            return CreatedAtAction(nameof(GetProductById), new { product.Id }, product);
        }

        [HttpPut]
        public IActionResult Update(Product product)
        {
            var success = _ProductData.Edit(product);
            return Ok(success);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var result = _ProductData.Delete(Id);

            return result
                ? Ok(true)
                : NotFound(false);
        }
    }
}
