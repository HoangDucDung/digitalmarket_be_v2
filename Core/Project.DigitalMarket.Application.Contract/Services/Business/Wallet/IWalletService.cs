using Project.DigitalMarket.Application.Contract.DTOs.Business.Wallet;

namespace Project.DigitalMarket.Application.Contract.Services.Business.Wallet
{
    /// <summary>
    /// Service quản lý các nghiệp vụ business (Application Logic) cho Ví điện tử
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// Lấy số dư hiện tại của người dùng.
        /// </summary>
        Task<decimal> GetBalanceAsync();

        /// <summary>
        /// Nạp tiền vào ví.
        /// </summary>
        Task TopUpAsync(decimal amount, string? description);

        /// <summary>
        /// Lấy lịch sử giao dịch.
        /// </summary>
        Task<List<WalletTransactionDTO>> GetTransactionsAsync(int page = 1, int pageSize = 10);
    }
}
