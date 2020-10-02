using System.Threading.Tasks;

namespace BooksWarehouse.Infrastructure
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}