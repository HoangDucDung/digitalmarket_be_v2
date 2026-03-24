using AutoMapper;
using Project.DigitalMarket.Application.Contract.DTOs.Auths;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using Project.DigitalMarket.Domain.Models.Auths;
using Project.DigitalMarket.Domain.Models.Business;

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
            CreateMap<DiscoveryRequestDto, ProductDiscoveryReq>();
            
            // Nếu cần phản hồi ngược: Domain (Req) -> Contract (DTO)
            CreateMap<SellerRegisterReq, SellerRegisterDto>();
        }
    }
}
