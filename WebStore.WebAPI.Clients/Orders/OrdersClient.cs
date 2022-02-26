using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mapper;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient Client)
            : base(Client, "api/orders")
        {
        }

        public async Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken cancel = default)
        {
            var model = new CreateOrderDTO()
            {
                Items = Cart.ToDTO(),
                Order = OrderModel,
            };

            var response = await PostAsync($"{Address}/{UserName}", model).ConfigureAwait(false);
            var order = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<OrderDTO>(cancellationToken: cancel)
                .ConfigureAwait(false);

            return order.FromDTO();
        }

        public async Task<Order?> GetOrderByIdAsync(int Id, CancellationToken cancel = default)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{Id}").ConfigureAwait(false);

            return order.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken cancel = default)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}").ConfigureAwait(false);

            return orders.FromDTO()!;
        }
    }
}
