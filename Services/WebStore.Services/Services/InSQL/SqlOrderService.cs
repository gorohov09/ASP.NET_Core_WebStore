using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;

namespace WebStore.Services.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;

        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public async Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken cancel = default)
        {
            var user = await _UserManager.FindByNameAsync(UserName).ConfigureAwait(false);

            if (user is null)
                throw new InvalidOperationException($"Пользователь с именем {UserName} не найден в БД");

            await using var transaction = await _db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);

            var order = new Order
            {
                User = user,
                Phone = OrderModel.Phone,
                Address = OrderModel.Address,
                Description = OrderModel.Description,
            };

            var products_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

            var cart_products = await _db.Products
                                        .Where(p => products_ids.Contains(p.Id))
                                        .ToArrayAsync(cancel)
                                        .ConfigureAwait(false);

            order.Items = Cart.Items.Join(
                                cart_products,
                                cart_item => cart_item.Product.Id,
                                cart_product => cart_product.Id,
                                (cart_item, cart_product) => new OrderItem
                                {
                                    Order = order,
                                    Product = cart_product,
                                    Price = cart_product.Price,
                                    Quantity = cart_item.Quantity,
                                }).ToArray();

            await _db.Orders.AddAsync(order).ConfigureAwait(false);

            await _db.SaveChangesAsync(cancel).ConfigureAwait(false);

            await transaction.CommitAsync().ConfigureAwait(false);

            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int Id, CancellationToken cancel = default)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.Id == Id)
                .ConfigureAwait(false);

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken cancel = default)
        {
            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .Where(o => o.User.UserName == UserName)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return orders;
        }
    }
}
