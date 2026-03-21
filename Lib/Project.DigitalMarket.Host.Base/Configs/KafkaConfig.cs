using Project.DigitalMarket.Domain.Share.Config;

namespace Project.DigitalMarket.Host.Base.Configs
{
    public class KafkaConfig : IKafkaConfig
    {
        public ICunsumerCustomConfig? Cunsumer { get; set; }

        public IProducerCustomConfig? Producer { get; set; }
    }

    public class CunsumerCustomConfig : ICunsumerCustomConfig
    {
        public string AutoOffsetReset { set; get; } = string.Empty;
        public string EnableAutoCommit { set; get; } = string.Empty;
        public string BootstrapServers { set; get; } = string.Empty;
        public string GroupId { set; get; } = string.Empty;
        public string Topic { set; get; } = string.Empty;
    }

    public class ProducerCustomConfig : IProducerCustomConfig
    {
        public string BootstrapServers { set; get; } = string.Empty;
        public string Topic { set; get; } = string.Empty;
    }
}
