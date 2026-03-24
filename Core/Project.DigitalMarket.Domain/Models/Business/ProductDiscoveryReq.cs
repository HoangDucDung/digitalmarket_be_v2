namespace Project.DigitalMarket.Domain.Models.Business
{
    public class ProductDiscoveryReq
    {
        public string Bundle { get; set; } = "daily_discover_main";
        public int Limit { get; set; } = 60;
        public int Offset { get; set; }
        public int ItemCard { get; set; } = 2;
        public bool NeedTab { get; set; }
        public string? ViewSessionId { get; set; }
    }
}
