using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient Client) : base(Client, WebAPIAddresses.Orders)
        {
        }

        public async Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken cancel = default)
        {
            var createOrderDTO = new CreateOrderDTO
            {
                Order = OrderModel,
                Items = Cart.ToDTO(),
            };
            var response = await PostAsync($"{Address}/{UserName}", createOrderDTO).ConfigureAwait(false);
            var orderDTO = await response!
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<OrderDTO>()
                .ConfigureAwait(false);
            return orderDTO.FromDTO()!;
        }

        public async Task<Order?> GetOrderByIdAsync(int Id, CancellationToken cancel = default)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{Id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken cancel = default)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}").ConfigureAwait(false);
            return orders!.FromDTO()!;
        }
    }
}
