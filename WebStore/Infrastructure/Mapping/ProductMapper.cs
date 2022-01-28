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

            return new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Section = product.Section.Name,
                Brand = product.Brand.Name
            };
        }

        public static Product? FromView(this ProductViewModel product)
        {
            if (product is null)
                return null;

            return new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };
        }

        public static IEnumerable<ProductViewModel?> ToView(this IEnumerable<Product?> products)
        {
            return products.Select(ToView); 
        }

        public static IEnumerable<Product?> FromView(this IEnumerable<ProductViewModel?> products)
        {
            return products.Select(FromView);
        }
    }
}
