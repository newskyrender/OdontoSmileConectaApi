namespace Integration.Api.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; } = 60;
        public int MaxRetryCount { get; set; } = 3;
        public int MaxRetryDelay { get; set; } = 30;
    }
}
