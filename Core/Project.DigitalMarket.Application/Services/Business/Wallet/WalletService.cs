using Project.DigitalMarket.Application.Contract.DTOs.Business.Wallet;
using Project.DigitalMarket.Application.Contract.Services.Business.Wallet;
using Project.DigitalMarket.Domain.Repositories.Auths.Wallet;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Domain.Managers.Business.Wallet;

namespace Project.DigitalMarket.Application.Services.Business.Wallet
{
    /// <summary>
    /// Triển khai Service quản lý nghiệp vụ business cho Ví
    /// </summary>
    internal sealed class WalletService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<WalletService>(lazyloadProvider), IWalletService
    {
        private IWalletManager _walletManager => _lazyloadProvider.LazyGetRequiredService<IWalletManager>();
        private IWalletTransactionRepository _transactionRepository => _lazyloadProvider.LazyGetRequiredService<IWalletTransactionRepository>();

        /// <summary>
        /// Lấy số dư hiện tại của người dùng đang đăng nhập
        /// </summary>
        public async Task<decimal> GetBalanceAsync()
        {
            var wallet = await _walletManager.GetOrCreateWalletAsync(UserId);
            return wallet.Balance;
        }

        /// <summary>
        /// Nạp tiền vào ví cho người dùng hiện tại
        /// </summary>
        public async Task TopUpAsync(decimal amount, string? description)
        {
            // Nghiệp vụ business: Kiểm tra số tiền đầu vào
            if (amount <= 0) throw new BusinessException(ErrorCode.InvalidAmount, "Số tiền nạp phải lớn hơn 0.");
            
            await _walletManager.ProcessTransactionAsync(UserId, amount, 
                WalletConstants.TransactionType.Deposit, 
                description ?? "Nạp tiền vào ví");
        }

        /// <summary>
        /// Lấy lịch sử giao dịch của người dùng hiện tại (Thông qua Repository)
        /// </summary>
        public async Task<List<WalletTransactionDTO>> GetTransactionsAsync(int page = 1, int pageSize = 10)
        {
            var transactions = await _transactionRepository.GetPagedByUserIdAsync(UserId, page, pageSize);
            return _mapper.Map<List<WalletTransactionDTO>>(transactions);
        }
    }
}
