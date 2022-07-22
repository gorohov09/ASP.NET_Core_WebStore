using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IViewComponentResult Invoke(string BrandId)
        {
            var brands = GetBrands();

            return View(new SelectableBrandsViewModel
            {
                Brands = brands,
                BrandId = int.TryParse(BrandId, out var id) ? id : (int?)null
            });
        }

        private IEnumerable<BrandViewModel> GetBrands()
        {
            return _ProductData.GetBrands()
                .OrderBy(b => b.Id)
                .Select(b => new BrandViewModel
                {
                    Id = b.Id,
                    Name = b.Name
                });
        }
    }
}
