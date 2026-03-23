using Project.DigitalMarket.Application.Contract.DTOs.Business;

namespace Project.DigitalMarket.Application.Contract.Services.Business
{
    /// <summary>
    /// Interface cho Service xử lý các nghiệp vụ liên quan đến Seller
    /// </summary>
    public interface ISellerService
    {
        /// <summary>
        /// Đăng ký thành Seller (Người bán)
        /// </summary>
        Task RegisterAsSellerAsync(Guid userId, SellerRegisterDto registerDto);
    }
}
