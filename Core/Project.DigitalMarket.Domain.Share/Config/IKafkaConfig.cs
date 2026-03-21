namespace Project.DigitalMarket.Domain.Share.Config
{
    public interface IKafkaConfig
    {
        public ICunsumerCustomConfig? Cunsumer { get; }

        public IProducerCustomConfig? Producer { get; }
    }

    public interface ICunsumerCustomConfig
    {
        public string AutoOffsetReset { get; }
        public string EnableAutoCommit { get; }
        public string BootstrapServers { get; }
        public string GroupId { get; }
        public string Topic { get; }
    }

    public interface IProducerCustomConfig
    {
        public string BootstrapServers { get; }
        public string Topic { get; }
    }
}
