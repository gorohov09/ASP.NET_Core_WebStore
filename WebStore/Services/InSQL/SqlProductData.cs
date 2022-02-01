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

        public Product CreateProduct(
            string Name, 
            int Order, 
            decimal Price, 
            string ImageUrl, 
            string Section, 
            string? Brand = null) //Получаем сырые данные
        {
            var section = _db.Sections.FirstOrDefault(s => s.Name == Section); //Пытаемся обнаружить секцию по имени

            if (section is null) //Если не нашли секцию, создаем ее
                section = new Section() { Name = Section };

            var brand = new Brand();

            if (Brand is not null) 
            {
                brand = _db.Brands.FirstOrDefault(b => b.Name == Brand); //Пытаемся обнаружить бренд по имени

                if (brand is null) //Если не нашли бренд, создаем его
                    brand = new Brand() { Name = Brand };
            }

            var product = new Product() //По ним формируем объект товара
            {
                Name = Name,
                Order = Order,
                Price = Price,
                ImageUrl = ImageUrl,
                Section = section,
                Brand = brand,
            };

            _db.Products.Add(product); //В момент добавления товара в базу данных, бренд и секция автоматически будут созданы

            _db.SaveChanges();

            return product; //Возвращаем товар
        }

        public Brand? GetBrandById(int Id)
        {
            return _db.Brands
                .Include(b => b.Products)
                .FirstOrDefault(b => b.Id == Id);
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

        public Section? GetSectionById(int Id)
        {
            return _db.Sections
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == Id);
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }
    }
}
