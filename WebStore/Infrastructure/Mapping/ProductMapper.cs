using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel? ToView(this Product? product)
        {
            if (product is null)
                return null;

            var product_viewmodel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Section = product.Section.Name,
                Brand = product.Brand.Name
            };

            return product_viewmodel;
        }

        public static Product? FromView(this ProductViewModel? viewmodel)
        {
            if (viewmodel is null)
                return null;

            var product = new Product()
            {
                Id = viewmodel.Id,
                Name = viewmodel.Name,
                Price = viewmodel.Price,
                ImageUrl = viewmodel.ImageUrl,
            };

            return product;
        }

        public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product?> products)
        {
            return products.Select(ToView);
        }

        public static IEnumerable<Product?> FromView(this IEnumerable<ProductViewModel?> viewmodels)
        {
            return viewmodels.Select(FromView);
        }
    }
}
