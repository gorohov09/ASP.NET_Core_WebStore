using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args); //�������� ����������� ����������

var services = builder.Services; //�������� ������� ������ ����������
services.AddControllersWithViews(opt =>
{
    opt.Conventions.Add(new TestConvention()); //���������� ����������
}); //����������(��������) MVC


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

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run(); //������ ����������
