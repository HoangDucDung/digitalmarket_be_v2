using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Application.Contract.DTOs.Business.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.Services.Business
{
    public interface IProductService
    {
        Task<DiscoveryResponseDto> GetDailyDiscoverAsync(DiscoveryRequestDto discoveryRequestDto);
    }

}
