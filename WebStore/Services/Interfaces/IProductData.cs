using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter? Filter = null);

        Product GetProductById(int Id);

        Section? GetSectionById(int Id);

        Brand? GetBrandById(int Id);

        Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null);

        bool Edit(Product product);

        bool Delete(int Id);
    }
}
