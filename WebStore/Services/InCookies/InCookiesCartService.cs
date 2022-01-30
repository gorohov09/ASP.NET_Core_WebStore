using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _ContextAccessor;

        private readonly IProductData _ProductData;

        private readonly string _CartName;

        private Cart Cart //Приватное свойство
        {
            get //Нужно взять cookies и десериализовать ее из json
            {
                var context = _ContextAccessor.HttpContext; //Берем контекст

                var cookies = context!.Response.Cookies; //Берем cookies

                var cart_cookies = context.Request.Cookies[_CartName]; //Извлекаем нашу собственную куки

                if (cart_cookies is null) //Если cookies нет, ее нужно добавить
                {
                    var cart = new Cart(); //Создаем корзину

                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart)); //Добавляем в куки объект корзины

                    return cart; //Возвращаем пустую корзину
                }

                ReplaceCart(cookies, cart_cookies); //Заменяем корзину
                
                return JsonConvert.DeserializeObject<Cart>(cart_cookies)!; //Десериализуем ее из строки
            }
            set //Сериализовать cart в json и добавить в cookies
            {
                //В куки сериализуем значение корзины
                ReplaceCart(_ContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// Подменяет одну корзину другой
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="cart"></param>
        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_CartName); //Удаляем существующую корзину

            cookies.Append(_CartName, cart); //Добавляем новую
        }

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _ContextAccessor = httpContextAccessor; //Получили http-контекст
            _ProductData = productData;

            var user = _ContextAccessor.HttpContext!.User;

            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"Web.Store.Cart{user_name}";
        }

        public void Add(int Id)
        {
            var cart = Cart; //Дессериализуем корзины

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id); //Пытаемся найти объект, который есть в корзине

            if (item is null)
            {
                cart.Items.Add(new CartItem() { ProductId = Id, Quantity = 1 });
            }
            else
            {
                item.Quantity += 1; //Инкрементируем кол-во
            }

            Cart = cart; //Сериализуем модифицированную корзину
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public void Decrement(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null)
                return;

            if (item.Quantity > 1)
                item.Quantity -= 1;

            if (item.Quantity == 1)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var cart = Cart;
            //Извлекаем товары идентификаторы которых нам нужны
            var products = _ProductData.GetProducts(new()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
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
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(p => p.ProductId == Id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            Cart = cart;
        }
    }
}
