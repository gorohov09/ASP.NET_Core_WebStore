using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var sections = _ProductData.GetSections();

            return Ok(sections.ToDTO());
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _ProductData.GetBrands();

            return Ok(brands.ToDTO());
        }

        [HttpGet("brands/{Id}")]
        public IActionResult GetBrandById(int Id)
        {
            var brand = _ProductData.GetBrandById(Id);

            if (brand is null)
                return NotFound();

            return Ok(brand.ToDTO());
        }

        [HttpGet("sections/{Id}")]
        public IActionResult GetSectionById(int Id)
        {
            var section = _ProductData.GetSectionById(Id);

            if (section is null)
                return NotFound();

            return Ok(section.ToDTO());
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter? Filter)
        {
            var products = _ProductData.GetProducts(Filter);

            return Ok(products.ToDTO());
        }

        [HttpGet("{Id}")]
        public IActionResult GetProductById(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            return Ok(product.ToDTO());
        }

        [HttpPost("new/{Name}")]
        public IActionResult CreateProduct(CreateProductDTO Model)
        {
            var product = _ProductData.CreateProduct(
                Model.Name, Model.Order, Model.Price, Model.ImageUrl, Model.Section, Model.Brand);

            if (product is null)
                return NotFound();

            return CreatedAtAction(nameof(GetProductById), new { product.Id }, product.ToDTO());
        }

        [HttpPut]
        public IActionResult EditProduct(ProductDTO product)
        {
            var success = _ProductData.Edit(product.FromDTO());

            return Ok(success);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteProduct(int Id)
        {
            var success = _ProductData.Delete(Id);

            return success
                ? Ok(true)
                : NotFound(false);
        }

    }
}
