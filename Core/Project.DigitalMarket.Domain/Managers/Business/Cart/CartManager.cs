using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Repositories.Business.Cart;
using Project.DigitalMarket.Domain.Repositories.Business.Product;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;

namespace Project.DigitalMarket.Domain.Managers.Business.Cart
{
    internal sealed class CartManager(ILazyloadProvider lazyloadProvider) : ManagerBase(lazyloadProvider), ICartManager
    {
        private ICartRepository _cartRepository => _lazyloadProvider.LazyGetRequiredService<ICartRepository>();
        private IProductRepository _productRepository => _lazyloadProvider.LazyGetRequiredService<IProductRepository>();

        public async Task AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var product = await _productRepository.GetProductDetailByIdAsync(productId);
            if (product == null)
                throw new BusinessException(ErrorCode.ProductNotAvailable, "Sản phẩm không khả dụng.");

            if (product.SellerId == userId)
                throw new BusinessException(ErrorCode.CannotBuyOwnProduct, "Bạn không thể thêm sản phẩm của chính mình vào giỏ hàng.");

            var cartItem = await _cartRepository.GetCartItemByProductAsync(userId, productId);
            if (cartItem == null)
            {
                cartItem = new CartItemEntity
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    ReferencePrice = product.Variants.Where(v => v.IsActive).OrderBy(v => v.Price).Select(v => v.Price).FirstOrDefault()
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
    }
}
