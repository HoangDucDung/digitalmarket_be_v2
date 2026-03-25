namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Order
{
    public class DirectPurchaseReqDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
