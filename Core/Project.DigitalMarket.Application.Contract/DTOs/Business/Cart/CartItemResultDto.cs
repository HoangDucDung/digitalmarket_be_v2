namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Cart
{
    public class CartItemResultDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal ReferencePrice { get; set; }
        public bool IsSelected { get; set; }
    }
}
