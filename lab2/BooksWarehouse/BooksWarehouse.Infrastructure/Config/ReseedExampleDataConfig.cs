namespace BooksWarehouse.Infrastructure.Config
{
    public interface IReseedExampleDataConfig
    {
        public bool ReseedExampleData { get; set; }
    }
    public class ReseedExampleDataConfig : IReseedExampleDataConfig
    {
        public bool ReseedExampleData { get; set; }
    }
}