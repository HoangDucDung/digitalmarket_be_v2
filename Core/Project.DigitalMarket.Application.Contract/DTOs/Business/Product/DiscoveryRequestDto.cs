using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryRequestDto
    {
        // Xác định bộ dữ liệu (VD: main_page, category_page, flash_sale)
        public string Bundle { get; set; } = "daily_discover_main";

        // Loại UI của card sản phẩm (VD: 1 là loại nhỏ, 2 là loại lớn đầy đủ video)
        public int ItemCard { get; set; } = 2;

        // Số lượng sản phẩm muốn lấy
        public int Limit { get; set; } = 60;

        // Có cần trả về thông tin các Tab danh mục hay không
        public bool NeedTab { get; set; } = false;

        // Vị trí bắt đầu lấy (Phục vụ phân trang)
        public int Offset { get; set; } = 0;

        // ID phiên làm việc để thuật toán gợi ý không bị lặp lại sản phẩm cũ
        public string ViewSessionId { get; set; }
    }
}
