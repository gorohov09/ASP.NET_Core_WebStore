using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken cancel = default);

        Task<Order?> GetOrderByIdAsync(int Id, CancellationToken cancel = default);

        Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken cancel = default);
    }
}
