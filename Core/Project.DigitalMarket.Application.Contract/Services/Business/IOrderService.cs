using Project.DigitalMarket.Application.Contract.DTOs.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.Services.Business
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(Guid userId, CreateOrderDto dto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(Guid userId);
        Task CancelOrderAsync(Guid orderId, Guid userId);
    }
}
