using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DigitalMarket.Libs.DependencyInjection;
using Project.DigitalMarket.Domain.Models;

namespace Digitalmarket.Controller.Base.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DigitalBaseController : ControllerBase
    {
        protected ILazyloadProvider _lazyloadProvider;
        private UserContext? _userContext;

        public DigitalBaseController(ILazyloadProvider lazyloadProvider)
        {
            _lazyloadProvider = lazyloadProvider;
        }

        /// <summary>
        /// Thông tin người dùng hiện tại trích xuất từ JWT token
        /// </summary>
        protected UserContext UserContext
        {
            get
            {
                if (_userContext == null)
                {
                    _userContext = new UserContext
                    {
                        UserId = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : Guid.Empty,
                        Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                        FullName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                        Role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
                    };
                }
                return _userContext;
            }
        }
    }
}
