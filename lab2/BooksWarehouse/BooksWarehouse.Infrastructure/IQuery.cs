using System.Threading.Tasks;

namespace BooksWarehouse.Infrastructure
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query);
    }
}