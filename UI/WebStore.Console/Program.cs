using Microsoft.Extensions.DependencyInjection;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Products;

var service_collection = new ServiceCollection();
service_collection.AddHttpClient<IProductData, ProductsClient>(http => http.BaseAddress = new("http://localhost:5036"));

var provider = service_collection.BuildServiceProvider();

Console.WriteLine("Ожидание старта хоста WebAPI");
Console.ReadLine();

var product_data = provider.GetRequiredService<IProductData>();

var products = product_data.GetProducts();

foreach (var product in products)
{
    Console.WriteLine($"{product.Id}-{product.Name}-{product.Price}-{product.Section}-{product.Brand}");
}
