using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IOrderService _orderService => _lazyloadProvider.LazyGetRequiredService<IOrderService>();

        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(UserContext.UserId, dto);
            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết 1 đơn hàng
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id, UserContext.UserId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của user đang đăng nhập
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await _orderService.GetMyOrdersAsync(UserContext.UserId);
            return Ok(result);
        }

        /// <summary>
        /// Huỷ đơn hàng
        /// </summary>
        [HttpPut("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await _orderService.CancelOrderAsync(id, UserContext.UserId);
            return Ok(new { Message = "Đơn hàng đã được huỷ." });
        }
    }
}
