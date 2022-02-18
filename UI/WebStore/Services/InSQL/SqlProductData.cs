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

        public Product CreateProduct(string Name, 
            int Order, 
            decimal Price, 
            string ImageUrl, 
            string Section, 
            string? Brand = null)
        {
            var section = _db.Sections.FirstOrDefault(s => s.Name == Section);

            if (section is null)
            {
                section = new Section() { Name = Section };
            }

            var brand = _db.Brands.FirstOrDefault(b => b.Name == Brand);

            if (brand is null)
            {
                brand = new Brand() { Name = Brand };
            }

            var product = new Product()
            {
                Name = Name,
                Order = Order,
                Price = Price,
                ImageUrl = ImageUrl,
                Section = section,
                Brand = brand
            };

            _db.Products.Add(product);

            _db.SaveChanges();

            return product;
        }

        public bool Delete(int Id)
        {
            var db_product = GetProductById(Id);

            if (db_product is null)
                return false;

            _db.Remove(db_product);

            _db.SaveChanges();

            return true;
        }

        public bool Edit(Product product)
        {
            if (product is null)
                throw new ArgumentException(nameof(product));

            var db_product = GetProductById(product.Id);

            if (db_product is null)
                return false;

            db_product.Name = product.Name;
            db_product.Order = product.Order;
            db_product.Section = product.Section;
            db_product.Brand = product.Brand;
            db_product.ImageUrl = product.ImageUrl;
            db_product.Price = product.Price;
            db_product.SectionId = product.SectionId;
            db_product.BrandId = product.BrandId;

            _db.SaveChanges();

            return true;
        }

        public Brand? GetBrandById(int? Id)
        {
            var brand = _db.Brands
                .Include(b => b.Products)
                .FirstOrDefault(b => b.Id == Id);

            return brand;
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

        public Section GetSectionById(int Id)
        {
            var section = _db.Sections
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == Id);

            return section;
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }


    }
}
