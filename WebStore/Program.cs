using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //Создание построителя приложения

var services = builder.Services; //Получили сервисы нашего приложения
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашения
}); //Подключили(Добавили) MVC

services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();


var app = builder.Build(); //Сборка приложения 

//-----------------Конвейер обработки входного соединения---------------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleware")); //Создание собственного промежуточного ПО

app.UseStaticFiles(); //Добавляем в конвейер обработки использование статических файлов(html, css, js, img)

app.UseRouting(); //Добавление системы маршрутизации

app.UseMiddleware<TestMiddleware>(); //Добавление своего промежуточного П.О.

app.UseWelcomePage("/welcome");

//app.MapDefaultControllerRoute(); //Добавление маршрута по умолчанию

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run(); //Запуск приложения
