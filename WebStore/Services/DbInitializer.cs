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

        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Инициализация БД");
            if (RemoveBefore)
                await RemoveAsync(Cancel).ConfigureAwait(false);

            //await _db.Database.EnsureCreatedAsync();

            await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);

            await InitializerProductAsync(Cancel).ConfigureAwait(false);

            _Logger.LogInformation("Инициализация БД выполнена успешно");
        }

        public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Удаление БД...");
            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);
            if (result)
                _Logger.LogInformation("Удаление БД выполнено успешно");
            else
                _Logger.LogInformation("Удаление БД не требуется");
            return result;
        }

        private async Task InitializerProductAsync(CancellationToken Cancel)
        {
            if (_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация данных в БД не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация тестовых данных");

            _Logger.LogInformation("Добавление секций");
            await using(await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);

            }

            _Logger.LogInformation("Добавление брендов");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);

            }

            _Logger.LogInformation("Добавление товаров");
            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);

            }

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Employees.AddRangeAsync(TestData.Employees, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON", Cancel);

                await _db.SaveChangesAsync(Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Инициализация тестовых данных выполнена успешно");
        }
    }
}
