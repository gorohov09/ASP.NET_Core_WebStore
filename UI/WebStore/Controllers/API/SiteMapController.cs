using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    [Route("api/sitemap")]
    [ApiController]
    public class SiteMapController : ControllerBase
    {
        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var nodes = new List<SitemapNode>()
            {
                new(Url.Action("Index", "Home")),
                new(Url.Action("ConfiguredAction", "Home")),
                new(Url.Action("Blog", "Blogs")),
                new(Url.Action("Index", "WebAPI")),
                new(Url.Action("Index", "Catalog"))
            };

            nodes.AddRange(ProductData.GetSections().Select(s => new SitemapNode(Url.Action("Index", "Catalog", new {SectionId = s.Id}))));
            nodes.AddRange(ProductData.GetBrands().Select(b => new SitemapNode(Url.Action("Index", "Catalog", new { BrandId = b.Id }))));
            nodes.AddRange(ProductData.GetProducts().Select(p => new SitemapNode(Url.Action("Index", "Catalog", new { ProductId = p.Id }))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
