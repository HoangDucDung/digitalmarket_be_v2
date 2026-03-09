namespace Project.DigitalMarket.Host.Base.Configs
{
    public class KafkaConfig
    {
        public CunsumerCustomConfig? Cunsumer { get; set; }

        public ProducerCustomConfig? Producer { get; set; }
    }

    public class CunsumerCustomConfig
    {
        public string AutoOffsetReset { set; get; } = string.Empty;
        public string EnableAutoCommit { set; get; } = string.Empty;
        public string BootstrapServers { set; get; } = string.Empty;
        public string GroupId { set; get; } = string.Empty;
        public string Topic { set; get; } = string.Empty;
    }

    public class ProducerCustomConfig
    {
        public string BootstrapServers { set; get; } = string.Empty;
        public string Topic { set; get; } = string.Empty;
    }
}
