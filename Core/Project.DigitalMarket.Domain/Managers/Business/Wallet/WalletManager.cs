using Microsoft.AspNetCore.Identity;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Domain.Repositories.Auths.Wallet;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Domain.Managers.Business.Wallet
{
    /// <summary>
    /// Triển khai Manager quản lý nghiệp vụ lõi cho Ví (Core Entity Logic)
    /// </summary>
    internal sealed class WalletManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), IWalletManager
    {
        private IWalletRepository _walletRepository => _lazyloadProvider.LazyGetRequiredService<IWalletRepository>();
        private IWalletTransactionRepository _transactionRepository => _lazyloadProvider.LazyGetRequiredService<IWalletTransactionRepository>();
        private UserManager<UserEntity> _userManager => _lazyloadProvider.LazyGetRequiredService<UserManager<UserEntity>>();

        /// <summary>
        /// Lấy hoặc tạo mới ví cho người dùng
        /// </summary>
        public async Task<WalletEntity> GetOrCreateWalletAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                // Verify user exists before creating a wallet (prevent FK conflict with old tokens)
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new BusinessException(ErrorCode.AccountNotFound, "Tài khoản không tồn tại. Vui lòng đăng ký mới hoặc đăng nhập lại.");
                }

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
    }
}
