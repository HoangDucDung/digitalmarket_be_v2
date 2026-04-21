using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Base;

namespace Project.DigitalMarket.Domain.Repositories.Business.Order
{
    public interface IOrderRepository : IRepositoryBase<OrderEntity>
    {
        /// <summary>
        /// Lấy danh sách đơn hàng của người mua có phân trang
        /// </summary>
        Task<List<OrderEntity>> GetPagedByBuyerIdAsync(Guid buyerId, int page, int pageSize);

        /// <summary>
        /// Lấy chi tiết đơn hàng kèm thông tin Items và Product
        /// </summary>
        Task<OrderEntity?> GetOrderDetailByIdAsync(Guid buyerId, Guid orderId);
    }
}
