using Project.DigitalMarket.Application.Contract.DTOs.Business.Order;
using Project.DigitalMarket.Application.Contract.Services.Business.Order;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Managers.Business.Order;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business.Order
{
    public class OrderService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IOrderService
    {
        private IOrderManager _orderManager => _lazyloadProvider.LazyGetRequiredService<IOrderManager>();

        public async Task<OrderResultDto> CheckoutCartAsync(CheckoutCartReqDto req)
        {
            var order = await _orderManager.CheckoutCartAsync(UserId, req.PaymentMethod, req.Note);
            return _mapper.Map<OrderResultDto>(order);
        }

        public async Task<OrderResultDto> DirectPurchaseAsync(DirectPurchaseReqDto req)
        {
            var order = await _orderManager.DirectPurchaseAsync(UserId, req.ProductId, req.Quantity, req.PaymentMethod, req.Note);
            return _mapper.Map<OrderResultDto>(order);
        }

        public async Task<List<OrderResultDto>> GetMyOrdersAsync()
        {
            var orders = await _orderManager.GetUserOrdersAsync(UserId);
            return _mapper.Map<List<OrderResultDto>>(orders);
        }

        public async Task<OrderResultDto> GetOrderDetailAsync(Guid orderId)
        {
            var order = await _orderManager.GetOrderDetailAsync(UserId, orderId);
            return _mapper.Map<OrderResultDto>(order);
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            await _orderManager.CancelOrderAsync(UserId, orderId);
        }
    }
}
