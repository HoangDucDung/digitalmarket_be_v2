namespace Project.DigitalMarket.Host.Base.Configs
{
    public class HangfireConfig
    {
        public string ConnectionString { get; set; } = string.Empty;

        public Dictionary<string, string> Authen { get; set; } = [];

    }
}
