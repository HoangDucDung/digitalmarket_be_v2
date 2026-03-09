namespace Project.DigitalMarket.Host.Base.Configs
{
    public class AuthConfig
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresTime { get; set; }
        public int RefreshTokenTime { get; set; }
    }
}