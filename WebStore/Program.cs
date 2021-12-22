using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //�������� ����������� ����������

var services = builder.Services; //�������� ������� ������ ����������
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //���������� ����������
}); //����������(��������) MVC

services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();//���������� ������ ������� ��� ������ � ������������
services.AddSingleton(<IProductData, InMemoryProductData>);


var app = builder.Build(); //������ ���������� 

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
