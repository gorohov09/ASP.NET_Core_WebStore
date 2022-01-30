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
            var product = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == Id);

            return product;
        }

        public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0) //Если у нас есть хоть что-то в массиве идентификаторов
            {
                query = query.Where(p => Filter.Ids.Contains(p.Id)); //Отбираем те товары, идентификаторы которых, есть в массиве
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
