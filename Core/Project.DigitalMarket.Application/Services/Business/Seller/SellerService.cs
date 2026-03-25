using Project.DigitalMarket.Application.Contract.DTOs.Business.Seller;
using Project.DigitalMarket.Application.Contract.Services.Business.Seller;
using Project.DigitalMarket.Domain.Managers.Business.Seller;
using Project.DigitalMarket.Domain.Models.Business.Seller;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business.Seller
{
    /// <summary>
    /// Service xử lý các nghiệp vụ liên quan đến Seller
    /// </summary>
    public class SellerService(ILazyloadProvider lazyloadProvider) : DigitalMarketServiceBase(lazyloadProvider), ISellerService
    {
        private ISellerManager _sellerManager => _lazyloadProvider.LazyGetRequiredService<ISellerManager>();

        /// <summary>
        /// Đăng ký thành Seller (Người bán)
        /// </summary>
        public async Task RegisterAsSellerAsync(Guid userId, SellerRegisterDto registerDto)
        {
            var req = _mapper.Map<SellerRegisterReq>(registerDto);
            await _sellerManager.RegisterAsSellerAsync(userId, req);
        }
    }
}
