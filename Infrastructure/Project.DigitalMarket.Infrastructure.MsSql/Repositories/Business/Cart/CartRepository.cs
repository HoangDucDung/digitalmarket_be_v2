using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Infrastructure.MsSql.Repositories.Base;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Infrastructure.MsSql.Repositories.Business.Cart
{
    internal sealed class CartRepository(ILazyloadProvider lazyloadProvider) : RepositoryBase<CartItemEntity>(lazyloadProvider), ICartRepository
    {
        public Task<List<CartItemEntity>> GetCartByUserIdAsync(Guid userId)
        {
            return GetByCondition(x => x.UserId == userId)
                .Include(x => x.Product)
                .ThenInclude(p => p.Images)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public Task<CartItemEntity?> GetCartItemAsync(Guid userId, Guid cartItemId)
        {
            return GetByCondition(x => x.Id == cartItemId && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public Task<CartItemEntity?> GetCartItemByProductAsync(Guid userId, Guid productId)
        {
            return GetByCondition(x => x.UserId == userId && x.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var items = await GetByCondition(x => x.UserId == userId).ToListAsync();
            foreach (var item in items)
                Delete(item);
            await SaveChangesAsync();
        }

        public async Task<List<CartItemEntity>> GetSelectedItemsWithProductByUserIdAsync(Guid userId)
        {
            return await GetByCondition(x => x.UserId == userId && x.IsSelected)
                .Include(x => x.Product)
                .ThenInclude(p => p.Variants)
                .ToListAsync();
        }
    }
}
