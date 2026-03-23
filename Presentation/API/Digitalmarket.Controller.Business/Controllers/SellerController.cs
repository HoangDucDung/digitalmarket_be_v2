using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Digitalmarket.Controller.Base.Controllers;

namespace Digitalmarket.Controller.Business.Controllers
{
    /// <summary>
    /// Controller xử lý các nghiệp vụ liên quan đến Seller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : DigitalBaseController
    {
        public SellerController(ILazyloadProvider lazyloadProvider) : base(lazyloadProvider)
        {
        }

        private ISellerService _sellerService => _lazyloadProvider.LazyGetRequiredService<ISellerService>();

        /// <summary>
        /// Đăng ký trở thành người bán (Seller)
        /// </summary>
        /// <param name="sellerDto">Thông tin KYC và tài chính</param>
        /// <returns>Trạng thái đăng ký</returns>
        /// <response code="200">Đăng ký thành công, hồ sơ đang chờ duyệt</response>
        /// <response code="401">Chưa đăng nhập</response>
        [HttpPost("register-seller")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterAsSeller([FromBody] SellerRegisterDto sellerDto)
        {
            await _sellerService.RegisterAsSellerAsync(UserContext.UserId, sellerDto);
            return Ok(new { Message = "Đăng ký bán hàng thành công. Hồ sơ của bạn đang được duyệt." });
        }
    }
}
