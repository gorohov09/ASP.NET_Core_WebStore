var builder = WebApplication.CreateBuilder(args); //Создание построителя приложения

var services = builder.Services; //Получили сервисы нашего приложения
services.AddControllersWithViews(); //Подключили(Добавили) MVC


var app = builder.Build(); //Сборка приложения 

//-----------------Конвейер обработки входного соединения---------------------------

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

app.UseStaticFiles(); //Добавляем в конвейер обработки использование статических файлов(html, css, js, img)

app.UseRouting(); //Добавление системы маршрутизации

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run(); //Запуск приложения
