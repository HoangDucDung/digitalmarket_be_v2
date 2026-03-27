using Project.DigitalMarket.Domain.Share.Config;

namespace Project.DigitalMarket.Host.Base.Configs
{
    public class VercelBlobConfig : IVercelBlobConfig
    {
        public string ReadWriteToken { get; set; } = string.Empty;
    }
}