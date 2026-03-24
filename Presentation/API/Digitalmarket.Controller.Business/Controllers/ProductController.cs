using Digitalmarket.Controller.Base.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Product.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ProductController(ILazyloadProvider lazyloadProvider) : DigitalBaseController(lazyloadProvider)
    {
        private IProductService _productService => _lazyloadProvider.LazyGetRequiredService<IProductService>();

        /// <summary>
        /// Lấy danh sách sản phẩm (có filter + phân trang)
        /// </summary>
        [HttpGet("daily-discover")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<DiscoveryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyDiscover([FromQuery] DiscoveryRequestDto discoveryRequestDto)
        {
            var response = new ApiResponse<DiscoveryResponseDto>();
            var result = await _productService.GetDailyDiscoverAsync(discoveryRequestDto);
            response.Data = result;
            return Ok(response);
        }

        /// <summary>
        /// Lấy chi tiết sản phẩm
        /// </summary>
        [HttpGet("detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ProductDetailResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductDetail([FromQuery] ProductDetailRequestDto requestDto)
        {
            var result = await _productService.GetProductDetailAsync(requestDto);
            if (result is null)
            {
                return NotFound(new { error = "Product not found" });
            }

            var response = new ApiResponse<ProductDetailResponseDto>
            {
                Data = result
            };

            return Ok(response);
        }
    }
}
