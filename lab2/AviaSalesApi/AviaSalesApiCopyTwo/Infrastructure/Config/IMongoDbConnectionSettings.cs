namespace AviaSalesApiCopyTwo.Infrastructure.Config
{
    public interface IMongoDbConnectionSettings
    {
        public string ReadonlyConnectionString { get; set; }
        public string WriteOnlyConnectionString { get; set; }
        public string Database { get; set; }
    }
    
    public class MongoDbConnectionSettings : IMongoDbConnectionSettings
    {
        public string ReadonlyConnectionString { get; set; }
        public string WriteOnlyConnectionString { get; set; }
        public string Database { get; set; }
    }
}