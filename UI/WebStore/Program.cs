using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InSQL;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Test.Persons;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args); //Создание построителя приложения

var services = builder.Services; //Получили сервисы нашего приложения
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //Добавление соглашения
}); //Подключили(Добавили) MVC

services.AddDbContext<WebStoreDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //Добавление сервиса для работы с БД
services.AddTransient<IDbInitializer, DbInitializer>(); //Добавление сервиса для инициализации БД

//services.AddScoped<IEmployeesData, SqlEmployeesData>(); //Добавление нашего сервиса для работы с сотрудниками
//services.AddScoped<IProductData, SqlProductData>(); //Добавление сервиса для работы с продуктами
services.AddScoped<ICartService, InCookiesCartService>();
//services.AddScoped<IOrderService, SqlOrderService>();
services.AddScoped<IUserService, SqlUserService>();

var configuration = builder.Configuration;

services.AddHttpClient<IValueService, ValuesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IPersonsService, PersonsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
services.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new(configuration["WebAPI"]));

services.AddIdentity<User, Role>() //Добавление системы Identity в наши сервисы
    .AddEntityFrameworkStores<WebStoreDB>()
    .AddDefaultTokenProviders();


services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);

});

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "MyWebStore.Gb";
    opt.Cookie.HttpOnly = true;

    //opt.Cookie.Expiration = TimeSpan.FromDays(10);
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;
});

var app = builder.Build(); //Сборка приложения 

//-----------------Инициализация БД-------------------------------------------------//
await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(false);
}

//-----------------Конвейер обработки входного соединения---------------------------//

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleware")); //Создание собственного промежуточного ПО

app.UseStaticFiles(); //Добавляем в конвейер обработки использование статических файлов(html, css, js, img)

app.UseRouting(); //Добавление системы маршрутизации

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<TestMiddleware>(); //Добавление своего промежуточного П.О.

app.UseWelcomePage("/welcome");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "/{controller=Home}/{action=Index}/{id?}");
});

//app.MapDefaultControllerRoute(); //Добавление маршрута по умолчанию


app.Run(); //Запуск приложения
