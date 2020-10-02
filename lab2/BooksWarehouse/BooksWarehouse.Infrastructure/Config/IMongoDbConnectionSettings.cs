namespace BooksWarehouse.Infrastructure.Config
{
    public interface IMongoDbConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}