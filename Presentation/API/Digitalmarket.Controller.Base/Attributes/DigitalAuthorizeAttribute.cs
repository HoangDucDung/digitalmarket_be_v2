using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.Extensions.Extensions;
using Project.DigitalMarket.Libs.Exceptions;
using System.Net;
using Project.DigitalMarket.Domain.Share.Constants.Auths;
using Project.DigitalMarket.Libs.Constants.ErrorCode;

namespace Digitalmarket.Controller.Base.Attributes
{
    /// <summary>
    /// Custom Attribute để kiểm tra quyền truy cập (Role)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DigitalAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Danh sách các Role được phép truy cập (phân cách bằng dấu phẩy)
        /// </summary>
        public string Roles { get; set; } = string.Empty;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Kiểm tra xem có cho phép truy cập ẩn danh không ([AllowAnonymous])
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute);
            if (allowAnonymous) return;

            // 2. Kiểm tra người dùng đã đăng nhập chưa
            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new AuthException("Unauthorized: Bạn cần đăng nhập để thực hiện thao tác này.", ErrorCode.Unauthorized);
            }

            // 3. Kiểm tra Role từ claim "Role" (chuỗi phân cách bởi dấu phẩy)
            if (Roles.HasValue())
            {
                var requiredRoles = Roles.Split(',').Select(r => r.Trim());

                // Lấy giá trị từ claim "Role"
                var userRolesClaim = user.FindFirst(AppClaimTypes.Role)?.Value ?? string.Empty;
                var userRoles = userRolesClaim.IsNullOrEmpty()
                    ? []
                    : userRolesClaim.Split(',').Select(r => r.Trim());

                // Nếu không có role nào của người dùng khớp với role yêu cầu
                if (!requiredRoles.Any(role => userRoles.Contains(role)))
                {
                    throw new AuthException("Forbidden: Bạn không có quyền truy cập.", ErrorCode.Forbidden)
                    {
                        StatusCode = HttpStatusCode.Forbidden
                    };
                }
            }

            // Bạn có thể thêm các logic kiểm tra tùy chỉnh khác tại đây:
            // - Kiểm tra trạng thái tài khoản (bị khóa, chưa kích hoạt...)
            // - Kiểm tra Permission cụ thể trong database
            // - Kiểm tra IP, Device ID...
        }
    }
}
