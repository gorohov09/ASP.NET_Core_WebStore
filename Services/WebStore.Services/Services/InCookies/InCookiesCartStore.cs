using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InCookies
{
    public class InCookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _ContextAccessor;
        private readonly string _CartName;

        public InCookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {
            _ContextAccessor = httpContextAccessor; //Получили http-контекст

            var user = _ContextAccessor.HttpContext!.User;

            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"Web.Store.Cart{user_name}";
        }
        public Cart Cart //Приватное свойство
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
    }
}
