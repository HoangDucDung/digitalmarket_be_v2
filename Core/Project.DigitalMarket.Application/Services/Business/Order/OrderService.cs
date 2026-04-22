using Project.DigitalMarket.Application.Contract.DTOs.Business.Order;
using Project.DigitalMarket.Application.Contract.Services.Business.Order;
using Project.DigitalMarket.Domain.Managers.Business.Order;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Constants.ErrorCode;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Domain.Share.Constants.Business;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Application.Services.Business.Order
{
    internal sealed class OrderService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase<OrderService>(lazyloadProvider), IOrderService
    {
        private IOrderManager _orderManager => _lazyloadProvider.LazyGetRequiredService<IOrderManager>();
        private IOrderRepository _orderRepository => _lazyloadProvider.LazyGetRequiredService<IOrderRepository>();

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
            var orders = await _orderRepository.GetPagedByBuyerIdAsync(UserId, 1, 50); // Default pagination for now
            return _mapper.Map<List<OrderResultDto>>(orders);
        }

        public async Task<OrderResultDto> GetOrderDetailAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderDetailByIdAsync(UserId, orderId);
            
            if (order == null) throw new BusinessException(ErrorCode.OrderNotFound, "Đơn hàng không tồn tại.");
            
            return _mapper.Map<OrderResultDto>(order);
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderDetailByIdAsync(UserId, orderId);
            if (order == null) throw new BusinessException(ErrorCode.OrderNotFound, "Đơn hàng không tồn tại.");
            if (order.Status != OrderConstants.Status.Pending) throw new BusinessException(ErrorCode.OnlyPendingOrderAllowed, "Chỉ có thể hủy đơn hàng đang chờ.");

            order.Status = OrderConstants.Status.Cancelled;
            order.UpdatedAt = GenerateExtentions.Now;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}
