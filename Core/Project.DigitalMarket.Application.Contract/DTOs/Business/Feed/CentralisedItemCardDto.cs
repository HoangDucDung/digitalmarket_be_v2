using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business
{
    public class CentralisedItemCardDto
    {
        // Dữ liệu thô (ID, ShopID, Giá gốc...)
        public object ItemData { get; set; }

        // Dữ liệu đã format sẵn để Frontend chỉ việc "đổ" ra (Tên, Ảnh, Giá hiển thị...)
        public object ItemCardDisplayedAsset { get; set; }
    }
}
