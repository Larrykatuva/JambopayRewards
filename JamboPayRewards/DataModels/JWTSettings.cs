namespace JamboPayRewards.DataModels
{
    /// <summary>
    /// Container to store JWT setting from the configuration file
    /// </summary>
    public class JWTSettings
    {
        public string  SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}