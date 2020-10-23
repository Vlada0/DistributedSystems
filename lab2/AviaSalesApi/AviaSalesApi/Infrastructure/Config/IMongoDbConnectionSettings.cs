namespace AviaSalesApi.Infrastructure.Config
{
    public interface IMongoDbConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
    
    public class MongoDbConnectionSettings : IMongoDbConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}