using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders([FromServices] IOrderService OrderService)
        {
            var user_name = User!.Identity!.Name;

            var orders = await OrderService.GetUserOrdersAsync(user_name);

            var orders_model = orders.Select(order => new UserOrderViewModel
            {
                Id = order.Id,
                Address = order.Address,
                Date = order.Date,
                Description = order.Description,
                Phone = order.Phone,
                TotalPrice = order.TotalPrice
            });

            return View(orders_model);
        }
    }
}
