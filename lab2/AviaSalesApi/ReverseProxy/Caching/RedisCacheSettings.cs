namespace ReverseProxy.Caching
{
    public class RedisCacheSettings
    {
        public bool IsEnabled { get; set; }
        public string ConnectionsString { get; set; }
    }
}