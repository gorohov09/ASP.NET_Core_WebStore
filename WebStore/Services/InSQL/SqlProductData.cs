using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands;
        }

        public Product? GetProductById(int Id)
        {
            return _db.Products
                .Include(p => p.Brand)
                .Include(s => s.Section)
                .FirstOrDefault(p => p.Id == Id);
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0)
            {
                query = query.Where(product => Filter.Ids!.Contains(product.Id));
            }
            else
            {
                if (Filter?.SectionId != null)
                    query = query.Where(p => p.SectionId == Filter.SectionId);
                if (Filter?.BrandId != null)
                    query = query.Where(p => p.BrandId == Filter.BrandId);
            }

            return query;

        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }
    }
}
