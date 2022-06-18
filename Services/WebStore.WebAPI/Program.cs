using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);


//---------������������ ������� ��������� ������������--------------------//
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt =>
{
    const string webstore_api_xml = "WebStore.WebAPI.xml";
    const string webstore_domain_xml = "WebStore.Domain.xml";
    const string debug_path = @"bin/Debug/net6.0";

    //opt.IncludeXmlComments("WebStore.WebAPI.xml");
    //opt.IncludeXmlComments("WebStore.Domain.xml");

    if (File.Exists(webstore_api_xml))
        opt.IncludeXmlComments(webstore_api_xml);
    else if (File.Exists(Path.Combine(debug_path, webstore_api_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, webstore_api_xml));

    if (File.Exists(webstore_domain_xml))
        opt.IncludeXmlComments(webstore_domain_xml);
    else if (File.Exists(Path.Combine(debug_path, webstore_domain_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, webstore_domain_xml));

});

services.AddDbContext<WebStoreDB>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))); //���������� ������� ��� ������ � ��
services.AddTransient<IDbInitializer, DbInitializer>(); //���������� ������� ��� ������������� ��

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

services.AddScoped<IEmployeesData, SqlEmployeesData>(); //���������� ������ ������� ��� ������ � ������������
services.AddScoped<IProductData, SqlProductData>(); //���������� ������� ��� ������ � ����������
services.AddScoped<IOrderService, SqlOrderService>();

var app = builder.Build();

//-------�������� �������------------------------//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
