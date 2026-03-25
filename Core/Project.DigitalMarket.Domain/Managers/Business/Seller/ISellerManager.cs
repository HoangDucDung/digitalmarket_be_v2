using Project.DigitalMarket.Domain.Models.Business.Seller;

namespace Project.DigitalMarket.Domain.Managers.Business.Seller
{
    /// <summary>
    /// Interface cho Manager xử lý logic nghiệp vụ về Seller
    /// </summary>
    public interface ISellerManager
    {
        /// <summary>
        /// Logic nghiệp vụ đăng ký bán hàng
        /// </summary>
        Task RegisterAsSellerAsync(Guid userId, SellerRegisterReq registerDto);
    }
}
