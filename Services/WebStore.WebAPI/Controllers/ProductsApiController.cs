using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;
        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

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

        [HttpGet("sections/{Id}")]
        public IActionResult GetSectionById(int Id)
        {
            var section = _ProductData.GetSectionById(Id);
            return section == null ? NotFound() : Ok(section.ToDTO()); 
        }

        [HttpGet("brands/{Id}")]
        public IActionResult GetBrandById(int Id)
        {
            var brand = _ProductData.GetBrandById(Id);
            return brand == null ? NotFound() : Ok(brand.ToDTO());  
        }

        [HttpPost]
        public IActionResult GetProducts(ProductFilter? Filter = null)
        {
            var products = _ProductData.GetProducts(Filter);
            return Ok(products.ToDTO());
        }

        [HttpGet("{Id}")]
        public IActionResult GetProductById(int Id)
        {
            var product = _ProductData.GetProductById(Id);
            return product == null ? NotFound() : Ok(product.ToDTO());
        }

        [HttpPost("new")]
        public IActionResult CreateProduct(CreateProductDTO productDTO)
        {
            var product = _ProductData.CreateProduct(
                productDTO.Name,
                productDTO.Order,
                productDTO.Price,
                productDTO.ImageUrl,
                productDTO.Section,
                productDTO.Brand);

            return product == null ? NotFound() : Ok(product.ToDTO());
        }

        [HttpPut]
        public IActionResult UpdateProduct(ProductDTO product)
        {
            var success = _ProductData.Edit(product.FromDTO()!);
            return Ok(success);
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteProduct(int Id)
        {
            var success = _ProductData.Delete(Id);
            return Ok(success);
        }
    }
}
