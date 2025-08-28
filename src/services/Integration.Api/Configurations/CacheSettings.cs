namespace Integration.Api.Configurations
{
    public class CacheSettings
    {
        public string RedisConnectionString { get; set; }
        public string InstanceName { get; set; } = "OdontoSmileConecta";
        public int DefaultExpirationMinutes { get; set; } = 30;
    }
}
