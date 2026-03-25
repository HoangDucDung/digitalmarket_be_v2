namespace Project.DigitalMarket.Application.Contract.DTOs.Business
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
        public string? ShippingAddress { get; set; }
    }
}
