using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ProductController(ILazyloadProvider lazyloadProvider) : DigitalBaseController<ProductController>(lazyloadProvider)
    {
        private IProductService _productService => _lazyloadProvider.LazyGetRequiredService<IProductService>();

        /// <summary>
        /// Lấy danh sách sản phẩm (có filter + phân trang)
        /// </summary>
        [HttpGet("daily-discover")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<DiscoveryResDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyDiscover(
            [FromQuery] int limit = 60,
            [FromQuery] int offset = 0,
            [FromQuery] bool needTab = false,
            [FromQuery] string viewSessionId = "",
            [FromQuery] string keyword = "")
        {
            var discoveryRequestDto = new DiscoveryReqDto
            {
                Limit = limit,
                Offset = offset,
                NeedTab = needTab,
                ViewSessionId = viewSessionId,
                Keyword = keyword
            };

            var response = new ApiResponse<DiscoveryResDto>();
            var result = await _productService.GetDailyDiscoverAsync(discoveryRequestDto);
            response.Data = result;
            return Ok(response);
        }

        /// <summary>
        /// Lấy chi tiết sản phẩm
        /// </summary>
        [HttpGet("detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductDetail([FromQuery] ProductDetailReqDto requestDto)
        {
            var result = await _productService.GetProductDetailAsync(requestDto);
            if (result is null)
            {
                return NotFound(new { error = "Product not found" });
            }

            var response = new ApiResponse<ProductDetailResDto>
            {
                Data = result
            };

            return Ok(response);
        }

        /// <summary>
        /// Thêm sản phẩm (tạo mới) - seller hiện tại
        /// </summary>
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ProductCreateResDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateReqDto req)
        {
            var result = await _productService.AddProductAsync(req);
            return Ok(new ApiResponse<ProductCreateResDto> { Data = result });
        }

        /// <summary>
        /// Sửa sản phẩm (patch) - seller hiện tại
        /// </summary>
        [HttpPut("update")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateReqDto req)
        {
            var updated = await _productService.UpdateProductAsync(req);
            return Ok(new ApiResponse<bool> { Data = updated });
        }

        /// <summary>
        /// Xóa (soft-delete) sản phẩm - seller hiện tại
        /// </summary>
        [HttpDelete("remove/{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var deleted = await _productService.DeleteProductAsync(productId);
            return Ok(new ApiResponse<bool> { Data = deleted });
        }

        /// <summary>
        /// Xóa (soft-delete) sản phẩm theo ItemId (query) — seller hiện tại
        /// </summary>
        [HttpDelete("delete")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProductByItemId([FromQuery] Guid itemId)
        {
            var deleted = await _productService.DeleteProductByItemIdAsync(itemId);
            return Ok(new ApiResponse<bool> { Data = deleted });
        }
    }
}
