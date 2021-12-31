using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;

        private readonly ILogger _Logger;

        public DbInitializer(WebStoreDB db, ILogger Logger)
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

            _Logger.LogInformation("Добавление секций");

            //Делаем транзацию, чтобы добавлялись все секции/бренды/продукты
            await using (await _db.Database.BeginTransactionAsync(cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", cancel);
                await _db.SaveChangesAsync(cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", cancel);

                await _db.Database.CommitTransactionAsync(cancel);
            }

            _Logger.LogInformation("Добавление брендов");
            await using (await _db.Database.BeginTransactionAsync(cancel))
            {
                await _db.Brands.AddRangeAsync(TestData.Brands, cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", cancel);
                await _db.SaveChangesAsync(cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", cancel);

                await _db.Database.CommitTransactionAsync(cancel);
            }

            _Logger.LogInformation("Добавление продуктов");
            await using (await _db.Database.BeginTransactionAsync(cancel))
            {
                await _db.Products.AddRangeAsync(TestData.Products, cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", cancel);
                await _db.SaveChangesAsync(cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", cancel);

                await _db.Database.CommitTransactionAsync(cancel);
            }

            _Logger.LogInformation("Инициализация тестовых данных выполнена успешно");
        }
    }
}
