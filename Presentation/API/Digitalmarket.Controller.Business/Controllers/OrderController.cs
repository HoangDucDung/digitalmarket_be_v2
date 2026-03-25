using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Order;
using Project.DigitalMarket.Application.Contract.Services.Business.Order;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IOrderService _orderService => _lazyloadProvider.LazyGetRequiredService<IOrderService>();

        /// <summary>
        /// Thanh toán giỏ hàng
        /// </summary>
        [HttpPost("checkout")]
        [ProducesResponseType(typeof(ApiResponse<OrderResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Checkout([FromBody] CheckoutCartReqDto req)
        {
            var result = await _orderService.CheckoutCartAsync(req);
            return Ok(new ApiResponse<OrderResultDto> { Data = result });
        }

        /// <summary>
        /// Mua ngay
        /// </summary>
        [HttpPost("direct-purchase")]
        [ProducesResponseType(typeof(ApiResponse<OrderResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DirectPurchase([FromBody] DirectPurchaseReqDto req)
        {
            var result = await _orderService.DirectPurchaseAsync(req);
            return Ok(new ApiResponse<OrderResultDto> { Data = result });
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của tôi
        /// </summary>
        [HttpGet("my-orders")]
        [ProducesResponseType(typeof(ApiResponse<List<OrderResultDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await _orderService.GetMyOrdersAsync();
            return Ok(new ApiResponse<List<OrderResultDto>> { Data = result });
        }

        /// <summary>
        /// Lấy chi tiết đơn hàng
        /// </summary>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(ApiResponse<OrderResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderDetail(Guid orderId)
        {
            var result = await _orderService.GetOrderDetailAsync(orderId);
            return Ok(new ApiResponse<OrderResultDto> { Data = result });
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        [HttpPost("{orderId}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await _orderService.CancelOrderAsync(orderId);
            return Ok(new ApiResponse<bool> { Data = true });
        }
    }
}
