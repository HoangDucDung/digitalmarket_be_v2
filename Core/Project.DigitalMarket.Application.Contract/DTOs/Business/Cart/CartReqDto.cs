namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Cart
{
    public class AddToCartReqDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateQuantityReqDto
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
