using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths.Wallet;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Managers.Auths.Wallet
{
    /// <summary>
    /// Triển khai Manager quản lý nghiệp vụ lõi cho Ví (Core Entity Logic)
    /// </summary>
    public class WalletManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IWalletManager
    {
        private IWalletRepository _walletRepository => _lazyloadProvider.LazyGetRequiredService<IWalletRepository>();
        private IWalletTransactionRepository _transactionRepository => _lazyloadProvider.LazyGetRequiredService<IWalletTransactionRepository>();

        /// <summary>
        /// Lấy hoặc tạo mới ví cho người dùng
        /// </summary>
        public async Task<WalletEntity> GetOrCreateWalletAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            if (wallet == null)
            {
                wallet = new WalletEntity
                {
                    UserId = userId,
                    Balance = 0,
                    Status = WalletConstants.WalletStatus.Active
                };
                await _walletRepository.AddAsync(wallet);
                await _walletRepository.SaveChangesAsync();
            }
            return wallet;
        }

        /// <summary>
        /// Xử lý nghiệp vụ lõi cho một giao dịch (Kiểm tra trạng thái, số dư, cập nhật balance và lưu log)
        /// </summary>
        public async Task ProcessTransactionAsync(Guid userId, decimal amount, string type, string description, string? referenceId = null)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            
            // Core checks: Kiểm tra trạng thái ví
            if (wallet.Status != WalletConstants.WalletStatus.Active)
                throw new BusinessException(ErrorCode.WalletLocked, "Ví của bạn đang bị khóa.");

            // Core checks: Kiểm tra số dư nếu là giao dịch trừ tiền
            if (amount < 0 && wallet.Balance < Math.Abs(amount))
                throw new BusinessException(ErrorCode.InsufficientBalance, "Số dư không đủ.");

            // Atomic state update: Cập nhật số dư
            wallet.Balance += amount;
            wallet.UpdatedAt = GenerateExtentions.Now;

            // Log transaction: Tạo bản ghi lịch sử giao dịch
            var transaction = new WalletTransactionEntity
            {
                WalletId = wallet.UserId,
                Amount = amount,
                Type = type,
                Status = WalletConstants.TransactionStatus.Completed,
                Description = description,
                ReferenceId = referenceId,
                CreatedAt = GenerateExtentions.Now
            };

            await _transactionRepository.AddAsync(transaction);
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Truy vấn lịch sử giao dịch của người dùng
        /// </summary>
        public async Task<List<WalletTransactionEntity>> GetTransactionsAsync(Guid userId, int page = 1, int pageSize = 10)
        {
            var wallet = await GetOrCreateWalletAsync(userId);
            return await _transactionRepository.GetByCondition(x => x.WalletId == wallet.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
