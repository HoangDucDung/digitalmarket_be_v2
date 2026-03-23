using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.Services.Business;
using Project.DigitalMarket.Domain.Managers.Business;
using Project.DigitalMarket.Domain.Models.Business;
using Project.DigitalMarket.Libs.DependencyInjection;

namespace Project.DigitalMarket.Application.Services.Business
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
