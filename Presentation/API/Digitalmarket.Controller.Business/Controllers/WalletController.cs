using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Application.Contract.Services.Business.Wallet;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Digitalmarket.Controller.Business.Controllers
{
    /// <summary>
    /// Controller quản lý các thao tác liên quan đến Ví điện tử và Giao dịch
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController(ILazyloadProvider lazyloadProvider) : ControllerBase
    {
        private IWalletService _walletService => lazyloadProvider.LazyGetRequiredService<IWalletService>();

        /// <summary>
        /// Xem số dư ví của tài khoản hiện tại
        /// </summary>
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var balance = await _walletService.GetBalanceAsync();
            return Ok(new { balance });
        }

        /// <summary>
        /// Thực hiện nạp tiền vào ví
        /// </summary>
        /// <param name="request">Thông tin số tiền và nội dung nạp</param>
        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] WalletTopUpRequest request)
        {
            await _walletService.TopUpAsync(request.Amount, request.Description);
            return Ok(new { message = "Nạp tiền thành công." });
        }

        /// <summary>
        /// Lấy danh sách lịch sử giao dịch của người dùng
        /// </summary>
        /// <param name="page">Trang hiện tại (Mặc định: 1)</param>
        /// <param name="pageSize">Kích thước trang (Mặc định: 10)</param>
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var transactions = await _walletService.GetTransactionsAsync(page, pageSize);
            return Ok(transactions);
        }
    }

    /// <summary>
    /// Request nạp tiền vào ví
    /// </summary>
    public class WalletTopUpRequest
    {
        /// <summary>
        /// Số tiền muốn nạp
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Nội dung/Ghi chú nạp tiền
        /// </summary>
        public string? Description { get; set; }
    }
}
