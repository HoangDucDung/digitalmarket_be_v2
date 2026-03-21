namespace Project.DigitalMarket.Domain.Share.Config
{
    public interface IAuthConfig
    {
        public string SecretKey { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int ExpiresTime { get; }
        public int RefreshTokenTime { get; }
    }
}
