using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;

        private readonly ILogger<DbInitializer> _Logger;

        private readonly UserManager<User> _UserManager;

        private readonly RoleManager<Role> _RoleManager;

        public DbInitializer(WebStoreDB db,
            ILogger<DbInitializer> Logger,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager)
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken cancel = default)
        {
            _Logger.LogInformation("Инициализация БД");

            if (RemoveBefore)
                await RemoveAsync(cancel).ConfigureAwait(false);

            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false); //Запускает процесс создания БД и перевод ее в последнее состояние

            await InitializerProductAsync(cancel).ConfigureAwait(false);

            await InitializeEmployeesAsync(cancel).ConfigureAwait(false);

            await InitializeIdentityAsync(cancel).ConfigureAwait(false);

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

        private async Task InitializeIdentityAsync(CancellationToken cancel)
        {
            _Logger.LogInformation("Инициализация данных системы Identity");

            var timer = Stopwatch.StartNew();

            if (await _RoleManager.RoleExistsAsync(Role.Administrators))
            {
                _Logger.LogInformation($"Роль {Role.Administrators} существует в БД");
            }
            else
            {
                _Logger.LogInformation($"Роль {Role.Administrators} не существует в БД");

                await _RoleManager.CreateAsync(new Role() { Name = Role.Administrators});

                _Logger.LogInformation($"Роль {Role.Administrators} создана");
            }

            if (await _RoleManager.RoleExistsAsync(Role.Users))
            {
                _Logger.LogInformation($"Роль {Role.Users} существует в БД");
            }
            else
            {
                _Logger.LogInformation($"Роль {Role.Users} не существует в БД");

                await _RoleManager.CreateAsync(new Role() { Name = Role.Users });

                _Logger.LogInformation($"Роль {Role.Users} создана");
            }

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation($"Пользователь {User.Administrator} отсутствует в БД");

                var admin = new User()
                {
                    UserName = User.Administrator
                };

                var create_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (create_result.Succeeded)
                {
                    _Logger.LogInformation($"Пользователь {User.Administrator} создан успешно");

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation($"Пользователь {User.Administrator} к работе готов");
                }
                else
                {
                    var errors = create_result.Errors.Select(errors => errors.Description);

                    _Logger.LogInformation($"Учетная запись администратора не создана. Ошибки: {string.Join(", ", errors)}");

                    throw new InvalidOperationException($"Невозможно создать пользователя {User.Administrator} по причине: {string.Join(", ", errors)}");
                }
            }

            _Logger.LogInformation($"Данная система Identity успешно добавлена в БД за {timer.Elapsed.TotalSeconds} c");
        }
    }
}
