namespace FirmenpartnerBackend.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
        public TimeSpan RefreshTokenExpiryTimeFrame { get; set; }
    }
}
