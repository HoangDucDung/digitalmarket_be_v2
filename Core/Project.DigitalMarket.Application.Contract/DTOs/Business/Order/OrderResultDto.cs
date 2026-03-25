namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Order
{
    public class OrderResultDto
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ProcessingFee { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
        public string? BuyerNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItemResultDto> Items { get; set; } = new List<OrderItemResultDto>();
    }

    public class OrderItemResultDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string? DeliveryInfo { get; set; }
    }
}
