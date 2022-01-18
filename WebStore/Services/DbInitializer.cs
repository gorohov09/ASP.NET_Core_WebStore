using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;

        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(WebStoreDB db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken cancel = default)
        {
            _Logger.LogInformation("Инициализация БД");

            if (RemoveBefore)
                await RemoveAsync(cancel).ConfigureAwait(false);

            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false); //Запускает процесс создания БД и перевод ее в последнее состояние

            await InitializerProductAsync(cancel).ConfigureAwait(false);

            await InitializeEmployeesAsync(cancel).ConfigureAwait(false);

            _Logger.LogInformation("Инициализация БД выполнена успешно");
        }

        public async Task<bool> RemoveAsync(CancellationToken cancel = default)
        {
            _Logger.LogInformation("Удаление БД...");

            var result = await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

            if (result)
                _Logger.LogInformation("Удаление БД выполнено успешно");
            else
                _Logger.LogInformation("Удаление БД не требуется");

            return result;
        }

        private async Task InitializerProductAsync(CancellationToken cancel)
        {
            if (_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация данных не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация тестовых данных");

            var sections_pool = TestData.Sections.ToDictionary(s => s.Id);
            var brands_pool = TestData.Brands.ToDictionary(b => b.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
            {
                child_section.Parent = sections_pool[(int)child_section.ParentId!];
            }

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];

                if (product.BrandId is not null)
                {
                    product.Brand = brands_pool[(int)product.BrandId];
                }

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
            {
                brand.Id = 0;
            }

            await using( await _db.Database.BeginTransactionAsync(cancel))
            {
                await _db.AddRangeAsync(TestData.Sections, cancel);

                await _db.AddRangeAsync(TestData.Brands, cancel);

                await _db.AddRangeAsync(TestData.Products, cancel);

                await _db.SaveChangesAsync();

                await _db.Database.CommitTransactionAsync(cancel);
            }

            _Logger.LogInformation("Инициализация тестовых данных выполнена успешно");
        }

        private async Task InitializeEmployeesAsync(CancellationToken cancel)
        {
            if (await _db.Employees.AnyAsync())
            {
                _Logger.LogInformation("Инициализация тестовых данных не требуется");
                return;
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                TestData.Employees.ForEach(emp => emp.Id = 0);

                await _db.AddRangeAsync(TestData.Employees, cancel);

                await _db.SaveChangesAsync(cancel);

                await _db.Database.CommitTransactionAsync(cancel);
            }

            _Logger.LogInformation("Инициализация тестовых данных выполнена успешно");
        }
    }
}
