using Microsoft.EntityFrameworkCore;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;

namespace Project.DigitalMarket.Domain.Managers.Business.Cart
{
    public class CartManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), ICartManager
    {
        private ICartRepository _cartRepository => _lazyloadProvider.LazyGetRequiredService<ICartRepository>();
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        public async Task AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var product = await _productRepository.GetByCondition(x => x.Id == productId && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
            if (product == null)
                throw new BusinessException(ErrorCode.ProductNotAvailable, "Sản phẩm không khả dụng.");

            if (product.SellerId == userId)
                throw new BusinessException(ErrorCode.CannotBuyOwnProduct, "Bạn không thể thêm sản phẩm của chính mình vào giỏ hàng.");

            var cartItem = await _cartRepository.GetByCondition(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();
            if (cartItem == null)
            {
                cartItem = new CartItemEntity
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    ReferencePrice = product.SalePrice ?? product.OriginalPrice
                };
                await _cartRepository.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _cartRepository.Update(cartItem);
            }
            await _cartRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(Guid userId, Guid cartItemId, int quantity)
        {
            var cartItem = await _cartRepository.GetByCondition(x => x.Id == cartItemId && x.UserId == userId).FirstOrDefaultAsync();
            if (cartItem == null) throw new BusinessException(ErrorCode.CartItemNotFound, "Mục giỏ hàng không tồn tại.");

            cartItem.Quantity = quantity;
            _cartRepository.Update(cartItem);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid cartItemId)
        {
            var cartItem = await _cartRepository.GetByCondition(x => x.Id == cartItemId && x.UserId == userId).FirstOrDefaultAsync();
            if (cartItem != null)
            {
                _cartRepository.Delete(cartItem);
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task<List<CartItemEntity>> GetUserCartAsync(Guid userId)
        {
            return await _cartRepository.GetByCondition(x => x.UserId == userId)
                .Include(x => x.Product)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var items = await _cartRepository.GetByCondition(x => x.UserId == userId).ToListAsync();
            foreach (var item in items)
                _cartRepository.Delete(item);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
