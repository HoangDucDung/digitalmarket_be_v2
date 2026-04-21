using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Order;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Order
{
    internal sealed class OrderRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<OrderEntity>(lazyloadProvider), IOrderRepository
    {
        public Task<List<OrderEntity>> GetPagedByBuyerIdAsync(Guid buyerId, int page, int pageSize)
        {
            return GetByCondition(x => x.BuyerId == buyerId)
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<OrderEntity?> GetOrderDetailByIdAsync(Guid buyerId, Guid orderId)
        {
            return GetByCondition(x => x.Id == orderId && x.BuyerId == buyerId)
                .Include(x => x.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();
        }
    }
}
