using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InCookies;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //�������� ����������� ����������

var services = builder.Services; //�������� ������� ������ ����������
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //���������� ����������
}); //����������(��������) MVC

services.AddDbContext<WebStoreDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //���������� ������� ��� ������ � ��
services.AddTransient<IDbInitializer, DbInitializer>(); //���������� ������� ��� ������������� ��
services.AddScoped<IEmployeesData, SqlEmployeesData>(); //���������� ������ ������� ��� ������ � ������������
services.AddScoped<IProductData, SqlProductData>(); //���������� ������� ��� ������ � ����������
services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IOrderService, SqlOrderService>();

services.AddIdentity<User, Role>() //���������� ������� Identity � ���� �������
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
    endpoints.MapControllerRoute(  //�������� ������ �������, ������� ����� � �������
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute( //��� �������� �������, �� ������ ���� ����� ���������, ����� ����������� ��� ������,
        name: "default",          //������� �� ������ � ������ ��������
        pattern: "{controller=Home}/{action=Index}/{id?}"); 
});

//app.MapDefaultControllerRoute(); //���������� �������� �� ���������

app.Run(); //������ ����������
