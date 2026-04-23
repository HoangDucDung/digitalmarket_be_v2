using Digitalmarket.Controller.Base.Controllers;
using Project.DigitalMarket.Libs.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Application.Contract.DTOs.Business;
using Project.DigitalMarket.Domain.Models.Commons;
using Digitalmarket.Controller.Base.Attributes;

namespace Digitalmarket.Controller.Business.Controllers
{
    public class TestRoleController(ILazyloadProvider lazyloadProvider) : DigitalBaseController<TestRoleController>(lazyloadProvider)
    {
        /// <summary>
        /// Test API dành cho tất cả người dùng đã đăng nhập (Dùng base [Authorize])
        /// </summary>
        [HttpGet("test-all")]
        public IActionResult TestAll()
        {
            return Ok(new ApiResponse<UserContext> { Data = UserContext });
        }

        /// <summary>
        /// Test API chỉ dành cho Admin (Dùng custom [DigitalAuthorize])
        /// </summary>
        [HttpGet("test-admin")]
        [DigitalAuthorize(Roles = RoleConstants.Admin)]
        public IActionResult TestAdmin()
        {
            return Ok(new ApiResponse<UserContext> { Data = UserContext });
        }

        /// <summary>
        /// Test API chỉ dành cho Seller (Dùng custom [DigitalAuthorize])
        /// </summary>
        [HttpGet("test-seller")]
        [DigitalAuthorize(Roles = RoleConstants.Seller)]
        public IActionResult TestSeller()
        {
            return Ok(new ApiResponse<UserContext> { Data = UserContext });
        }

        /// <summary>
        /// Test API chỉ dành cho Customer (Dùng custom [DigitalAuthorize])
        /// </summary>
        [HttpGet("test-customer")]
        [DigitalAuthorize(Roles = RoleConstants.Customer)]
        public IActionResult TestCustomer()
        {
            return Ok(new ApiResponse<UserContext> { Data = UserContext });
        }
    }
}
