using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.InSQL;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //�������� ����������� ����������

var services = builder.Services; //�������� ������� ������ ����������
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //���������� ����������
}); //����������(��������) MVC

services.AddDbContext<WebStoreDB>(opt => opt
    .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //����������� � ��
services.AddTransient<IDbInitializer, DbInitializer>();

services.AddScoped<IEmployeesData, SqlEmployeesData>(); //���������� ������ ������� ��� ������ � ������������
services.AddScoped<IProductData, SqlProductData>();



var app = builder.Build(); //������ ���������� 

await using(var scope = app.Services.CreateAsyncScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync();
}

//-----------------�������� ��������� �������� ����������---------------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

//app.Map("/testpath", async context => await context.Response.WriteAsync("TestMiddleware")); //�������� ������������ �������������� ��

app.UseStaticFiles(); //��������� � �������� ��������� ������������� ����������� ������(html, css, js, img)

app.UseRouting(); //���������� ������� �������������

app.UseMiddleware<TestMiddleware>(); //���������� ������ �������������� �.�.

app.UseWelcomePage("/welcome");

//app.MapDefaultControllerRoute(); //���������� �������� �� ���������

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run(); //������ ����������
