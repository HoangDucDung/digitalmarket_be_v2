using Project.DigitalMarket.Domain.Entities;

namespace Project.DigitalMarket.Domain.Managers.Business.Wallet
{
    /// <summary>
    /// Manager quản lý các nghiệp vụ lõi (Core Entity Logic) xoay quanh Ví điện tử
    /// </summary>
    public interface IWalletManager
    {
        /// <summary>
        /// Lấy thông tin ví của người dùng. Nếu chưa có thì khởi tạo ví mới với số dư 0.
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <returns>Thực thể Ví điện tử</returns>
        Task<WalletEntity> GetOrCreateWalletAsync(Guid userId);

        /// <summary>
        /// Xử lý một giao dịch tài chính (Cập nhật số dư và ghi log lịch sử)
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="amount">Số tiền (Dương: Tăng, Âm: Giảm)</param>
        /// <param name="type">Loại giao dịch (Deposit, Payment...)</param>
        /// <param name="description">Mô tả giao dịch</param>
        /// <param name="referenceId">Mã tham chiếu (Ví dụ: OrderCode)</param>
        Task ProcessTransactionAsync(Guid userId, decimal amount, string type, string description, string? referenceId = null);
    }
}
