using System.Threading.Tasks;

namespace BooksWarehouse.Infrastructure
{
    public interface IMediator
    {
        Task DispatchAsync(ICommand command);
        Task<T> DispatchAsync<T>(IQuery<T> query);
    }
}