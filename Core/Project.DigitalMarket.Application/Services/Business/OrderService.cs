using Project.DigitalMarket.Application.Contract.DTOs.Orders;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Libs.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Services.Business
{
    public class OrderService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), IOrderService
    {
        Task IOrderService.CancelOrderAsync(Guid orderId, Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<OrderResponseDto> IOrderService.CreateOrderAsync(Guid userId, CreateOrderDto dto)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<OrderResponseDto>> IOrderService.GetMyOrdersAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<OrderResponseDto> IOrderService.GetOrderByIdAsync(Guid orderId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
