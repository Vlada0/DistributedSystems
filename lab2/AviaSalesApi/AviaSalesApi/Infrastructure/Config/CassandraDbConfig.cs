namespace AviaSalesApi.Infrastructure.Config
{
    public interface ICassandraDbConfig
    {
        public string Host { get; set; }
        public string KeySpace { get; set; }
    }
    
    public class CassandraDbConfig : ICassandraDbConfig
    {
        public string Host { get; set; }
        public string KeySpace { get; set; }
    }
}