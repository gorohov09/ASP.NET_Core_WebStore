using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //Создание построителя приложения

var services = builder.Services; //Получили сервисы нашего приложения
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашения
}); //Подключили(Добавили) MVC

services.AddDbContext<WebStoreDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //Добавление сервиса для работы с БД
services.AddTransient<IDbInitializer, DbInitializer>(); //Добавление сервиса для инициализации БД
services.AddScoped<IEmployeesData, SqlEmployeesData>(); //Добавление нашего сервиса для работы с сотрудниками
services.AddScoped<IProductData, SqlProductData>(); //Добавление сервиса для работы с продуктами


var app = builder.Build(); //Сборка приложения 

//-----------------Инициализация БД-------------------------------------------------//
await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync();
}

//-----------------Конвейер обработки входного соединения---------------------------//

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleware")); //Создание собственного промежуточного ПО

app.UseStaticFiles(); //Добавляем в конвейер обработки использование статических файлов(html, css, js, img)

app.UseRouting(); //Добавление системы маршрутизации

app.UseMiddleware<TestMiddleware>(); //Добавление своего промежуточного П.О.

//app.MapDefaultControllerRoute(); //Добавление маршрута по умолчанию

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run(); //Запуск приложения
