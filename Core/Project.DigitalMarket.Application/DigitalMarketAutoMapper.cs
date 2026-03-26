using AutoMapper;
using Project.DigitalMarket.Application.Contract.DTOs.Auths;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Cart;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Order;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Seller;
using Project.DigitalMarket.Domain.Entities.Business;
using Project.DigitalMarket.Domain.Models.Auths;
using Project.DigitalMarket.Domain.Models.Business.Product;
using Project.DigitalMarket.Domain.Models.Business.Seller;
using Project.DigitalMarket.Domain.Entities;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Wallet;

namespace Project.DigitalMarket.Application
{
    /// <summary>
    /// Cấu hình AutoMapper cho toàn hệ thống
    /// </summary>
    public class DigitalMarketAutoMapper : Profile
    {
        public DigitalMarketAutoMapper()
        {
            // Auth mapping: Domain -> Contract
            CreateMap<InfoToken, AuthResponseDto>();

            // Business mapping: Contract (DTO) -> Domain (Req)
            CreateMap<SellerRegisterDto, SellerRegisterReq>();
            CreateMap<DiscoveryReqDto, ProductDiscoveryReq>();
            
            // Cart mapping
            CreateMap<CartItemEntity, CartItemResultDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.Product.ImageUrl));

            // Order & Item mapping: Entity (Domain) -> Result (Contract)
            CreateMap<OrderEntity, OrderResultDto>();
            CreateMap<OrderItemEntity, OrderItemResultDto>();

            // Wallet mapping
            CreateMap<WalletTransactionEntity, WalletTransactionDTO>();

            // Nếu cần phản hồi ngược: Domain (Req) -> Contract (DTO)
            CreateMap<SellerRegisterReq, SellerRegisterDto>();
        }
    }
}
