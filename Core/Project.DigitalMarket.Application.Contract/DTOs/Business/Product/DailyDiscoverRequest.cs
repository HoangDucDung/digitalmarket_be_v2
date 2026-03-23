using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business
{
    public class DailyDiscoverRequest
    {
        [Range(1, 100)]
        public int Limit { get; set; } = 20;

        [Range(0, int.MaxValue)]
        public int Offset { get; set; } = 0;

        public string Bundle { get; set; } = "daily_discover_main";

        // Filter options
        public string CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinDiscount { get; set; }
        public string SortBy { get; set; } = "relevance"; // relevance | price | sold | rating
        public string SortOrder { get; set; } = "desc";
    }
}
