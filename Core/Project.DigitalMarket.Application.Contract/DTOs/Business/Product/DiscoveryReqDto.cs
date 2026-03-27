using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business.Product
{
    public class DiscoveryReqDto
    {
        // Xác ??nh b? d? li?u (VD: main_page, category_page, flash_sale)
        public string? Bundle { get; set; } = "daily_discover_main";

        // Lo?i UI c?a card s?n ph?m (VD: 1 là lo?i nh?, 2 là lo?i l?n ??y ?? video)
        public int? ItemCard { get; set; } = 2;

        // S? l??ng s?n ph?m mu?n l?y
        public int? Limit { get; set; } = 60;

        // Có c?n tr? v? thông tin các Tab danh m?c hay không
        public bool? NeedTab { get; set; } = false;

        // V? trí b?t ??u l?y (Ph?c v? phân trang)
        public int? Offset { get; set; } = 0;

        // ID phiên làm vi?c ?? thu?t toán g?i ý không b? l?p l?i s?n ph?m c?
        public string? ViewSessionId { get; set; } = string.Empty;

        // T? khóa tìm ki?m
        public string? Keyword { get; set; } = string.Empty;
    }
}
