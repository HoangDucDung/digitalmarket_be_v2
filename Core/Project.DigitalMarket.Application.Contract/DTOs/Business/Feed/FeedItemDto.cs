using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business
{
    public class FeedItemDto
    {
        public string Type { get; set; } = "product_card"; // product_card, ads_card, collection_card
        public CentralisedItemCardDto CentralisedItemCard { get; set; }
    }
}
