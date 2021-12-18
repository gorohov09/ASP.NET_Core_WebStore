namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;

        public TestMiddleware(RequestDelegate Next)
        {
            _Next = Next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Обработка информации из Context.Request

            await _Next(context); //Далее здесь работает оставшаяся часть конвейера

            

            // Доработка данных в Context.Response
        }
    }
}
