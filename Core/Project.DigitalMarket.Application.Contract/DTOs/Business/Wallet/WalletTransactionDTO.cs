namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Wallet
{
    /// <summary>
    /// DTO hiển thị thông tin lịch sử giao dịch ví
    /// </summary>
    public class WalletTransactionDTO
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public string? ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
