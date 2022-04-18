var builder = WebApplication.CreateBuilder(args);


//---------Конфигурация системы внедрения зависимостей--------------------//
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

//-------Конвейер запроса------------------------//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
