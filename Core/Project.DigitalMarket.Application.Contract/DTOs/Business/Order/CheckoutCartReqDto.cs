namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Order
{
    public class CheckoutCartReqDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
