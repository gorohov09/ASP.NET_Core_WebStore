using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InSQL
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

        public async Task<Order> CreateOrderAsync(
            string UserName, 
            CartViewModel Cart, 
            OrderViewModel OrderModel, 
            CancellationToken Cancel = default)
        {
            //Находим пользователя по UserName
            var user = await _UserManager.FindByNameAsync(UserName).ConfigureAwait(false); 

            if (user is null) //Если пользователь не найден, то плохо
                throw new InvalidOperationException($"Пользователь с именем {UserName} не найден в БД");

            //Запускаем транзакцию
            await using var transaction = await _db.Database.BeginTransactionAsync(Cancel).ConfigureAwait(false);

            var order = new Order //Создаем объект заказа и заполняем его
            {
                User = user,
                Address = OrderModel.Address,
                Phone = OrderModel.Phone,
                Description = OrderModel.Description,
            };

            //Массив идентфикаторов товаров из корзины
            var products_ids = Cart.Items.Select(item => item.Product.Id).ToArray();

            //Извлекаем все товары, фильтруем товары и получаем массив товаров
            var cart_products = await _db.Products
                .Where(p => products_ids.Contains(p.Id)) //Выбираем из БД только те товары, котоырые были указаны в корзине
                .ToArrayAsync(Cancel)
                .ConfigureAwait(false);

            order.Items = Cart.Items.Join( //Берем список записей в корзине и присоединяем к нему товары
                cart_products,
                cart_item => cart_item.Product.Id,
                cart_product => cart_product.Id,
                (cart_item, cart_product) => new OrderItem
                {
                    Order = order,
                    Product = cart_product,
                    Price = cart_product.Price, //Здесь может быть применена скидка
                    Quantity = cart_item.Quantity
                }).ToArray();

            await _db.Orders.AddAsync(order, Cancel).ConfigureAwait(false); //Сохраняем объект order в БД

            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false); //Сохраняем изменения

            await transaction.CommitAsync(Cancel).ConfigureAwait(false); //Применяем транзакцию

            return order; //Возвращаем наш объект заказа
        }

        public async Task<Order?> GetOrderById(int Id, CancellationToken Cancel = default)
        {
            var order = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(o => o.Id == Id) //Ищем заказ по идентификатору
                .ConfigureAwait(false);

            return order; //Возвращаем заказ
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
        {
            var orders = await _db.Orders //Берем базу данных, берем таблицу Orders
                .Include(o => o.User) //Добавляем информацию для каждому заказу по объекту User
                .Include(o => o.Items) //Добавляем информацию по элементам самого заказа
                .ThenInclude(item => item.Product) //Внутри каждого элемента загружаем данные по самому товару
                .Where(o => o.User.UserName == UserName) //Фильтруем
                .ToArrayAsync(Cancel) //Делаем массив
                .ConfigureAwait(false);

            return orders; //Возвращаем массив
        }
    }
}
