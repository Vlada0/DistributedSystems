namespace BooksWarehouse.Infrastructure.Config
{
    public class MongoDbConnectionSettings : IMongoDbConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}