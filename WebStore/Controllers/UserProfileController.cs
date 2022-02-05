using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
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
            var orders = await OrderService.GetUserOrdersAsync(User.Identity!.Name!);

            var orders_model = orders.Select(o => new UserOrderViewModel()
            {
                Id = o.Id,
                Address = o.Address,
                Date = o.Date,
                Description = o.Description,
                Phone = o.Phone,
                TotalPrice = o.TotalPrice,
            });

            return View(orders_model);
        }
    }
}
