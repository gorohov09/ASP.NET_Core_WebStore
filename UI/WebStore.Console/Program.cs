using Microsoft.Extensions.DependencyInjection;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Products;

var service_collection = new ServiceCollection();

service_collection.AddHttpClient<IProductData, ProductsClient>(http => http.BaseAddress = new Uri("http://localhost:5293"));

var provider = service_collection.BuildServiceProvider();

Console.WriteLine("Ождиаем старта хоста WebAPI");
Console.ReadLine();

var product_data = provider.GetRequiredService<IProductData>();

var products = product_data.GetProducts();

foreach (var product in products)
{
    Console.WriteLine("[{0,4}] {1} {2} {3}",
        product.Id, product.Name, product.Price, product.ImageUrl);
}
Console.ReadLine();