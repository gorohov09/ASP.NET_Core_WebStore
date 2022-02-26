using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mapper;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpGet("user/{UserName}")]
        public async Task<IActionResult> GetUserOrders(string UserName, CancellationToken Cancel = default)
        {
            var orders = await _OrderService.GetUserOrdersAsync(UserName, Cancel);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderById(int Id, CancellationToken Cancel = default)
        {
            var order = await _OrderService.GetOrderByIdAsync(Id, Cancel);
            if (order is null)
                return NotFound();

            return Ok(order.ToDTO());
        }

        [HttpPost("{UserName}")]
        public async Task<IActionResult> CreateOrder(string UserName, [FromBody]CreateOrderDTO Model)
        {
            var order = await _OrderService.CreateOrderAsync(UserName, Model.Items.ToCartView(), Model.Order);
            return CreatedAtAction(nameof(GetOrderById), new { order.Id }, order.ToDTO());
        }
    }
}
