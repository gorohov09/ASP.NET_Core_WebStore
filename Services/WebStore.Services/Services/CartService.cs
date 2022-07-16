using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using WebStore.Services.Services.InCookies;
using WebStore.ViewModels;

namespace WebStore.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _CartStore;
        private readonly IProductData _ProductData;

        public CartService(ICartStore CartStore, IProductData productData)
        {
            _CartStore = CartStore;
            _ProductData = productData;
        }

        public void Add(int Id)
        {
            var cart = _CartStore.Cart; //Дессериализуем корзины

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id); //Пытаемся найти объект, который есть в корзине

            if (item is null)
            {
                cart.Items.Add(new CartItem() { ProductId = Id, Quantity = 1 });
            }
            else
            {
                item.Quantity += 1; //Инкрементируем кол-во
            }

            _CartStore.Cart = cart; //Сериализуем модифицированную корзину
        }

        public void Clear()
        {
            var cart = _CartStore.Cart;

            cart.Items.Clear();

            _CartStore.Cart = cart;
        }

        public void Decrement(int Id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null)
                return;

            if (item.Quantity > 1)
                item.Quantity -= 1;

            if (item.Quantity == 1)
                cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var cart = _CartStore.Cart;
            //Извлекаем товары идентификаторы которых нам нужны
            var products = _ProductData.GetProducts(new()
            {
                Ids = _CartStore.Cart.Items.Select(i => i.ProductId).ToArray()
            });

            //Преобразуем товары в словарь(ключ - индентификатор, значение - ViewModel)
            var products_views = products.ToView().ToDictionary(p => p.Id);

            //Формируем нашу ViewModel

            return new()
            {
                Items = cart.Items
                    .Where(item => products_views.ContainsKey(item.ProductId))
                    .Select(item => (products_views[item.ProductId], item.Quantity))
            };

        }

        public void Remove(int Id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }
    }
}
