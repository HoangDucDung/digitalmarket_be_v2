using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Cart;
using Project.DigitalMarket.Application.Contract.Services.Business.Cart;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Cart.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class CartController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private ICartService _cartService => _lazyloadProvider.LazyGetRequiredService<ICartService>();

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartReqDto req)
        {
            await _cartService.AddToCartAsync(req.ProductId, req.Quantity);
            return Ok(new ApiResponse<bool> { Data = true });
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpPut("update-quantity")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityReqDto req)
        {
            await _cartService.UpdateQuantityAsync(req.CartItemId, req.Quantity);
            return Ok(new ApiResponse<bool> { Data = true });
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        [HttpDelete("remove/{cartItemId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return Ok(new ApiResponse<bool> { Data = true });
        }

        /// <summary>
        /// Lấy giỏ hàng của tôi
        /// </summary>
        [HttpGet("my-cart")]
        [ProducesResponseType(typeof(ApiResponse<List<CartItemResultDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyCart()
        {
            var result = await _cartService.GetMyCartAsync();
            return Ok(new ApiResponse<List<CartItemResultDto>> { Data = result });
        }

        /// <summary>
        /// Xóa sạch giỏ hàng
        /// </summary>
        [HttpDelete("clear")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return Ok(new ApiResponse<bool> { Data = true });
        }
    }
}
