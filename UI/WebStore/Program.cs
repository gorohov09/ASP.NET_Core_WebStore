using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Logging;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InSQL;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Test.Persons;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args); //�������� ����������� ����������

#region ������� ��������� ������� �����������
//builder.Logging.ClearProviders()
//    .AddConsole(opt => opt.LogToStandardErrorThreshold = LogLevel.Information)
//    .AddFilter("Microsoft", level => level >= LogLevel.Information); //������ ��� ������ ����, ��� Information, ����� ������������� ������ � ������ ��� ��������� - Microsoft
#endregion

builder.Logging.AddLog4Net();

builder.Host.UseSerilog((host, log) => log
    .ReadFrom.Configuration(host.Configuration));

var services = builder.Services; //�������� ������� ������ ����������
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //���������� ����������
}); //����������(��������) MVC

services.AddDbContext<WebStoreDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //���������� ������� ��� ������ � ��

services.AddTransient<IDbInitializer, DbInitializer>(); //���������� ������� ��� ������������� ��
services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IUserService, SqlUserService>();

var configuration = builder.Configuration;

services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IValueService, ValuesClient>()
    .AddTypedClient<IPersonsService, PersonsClient>()
    .AddTypedClient<IEmployeesData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>()
    .AddTypedClient<IOrderService, OrdersClient>();

services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>();


services.AddIdentity<User, Role>() //���������� ������� Identity � ���� �������
    //.AddEntityFrameworkStores<WebStoreDB>()
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

services.AddAutoMapper(Assembly.GetEntryAssembly()); //���������� AutoMapper

var app = builder.Build(); //������ ���������� 

//-----------------������������� ��-------------------------------------------------//
await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(false);
}

//-----------------�������� ��������� �������� ����������---------------------------//

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleware")); //�������� ������������ �������������� ��

app.UseStaticFiles(); //��������� � �������� ��������� ������������� ����������� ������(html, css, js, img)

app.UseRouting(); //���������� ������� �������������

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<TestMiddleware>(); //���������� ������ �������������� �.�.

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

//app.MapDefaultControllerRoute(); //���������� �������� �� ���������


app.Run(); //������ ����������
