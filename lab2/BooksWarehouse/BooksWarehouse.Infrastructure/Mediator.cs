using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable PossibleNullReferenceException

namespace BooksWarehouse.Infrastructure
{
    public sealed class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(ICommand command)
        {
            var type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            await handler.ExecuteAsync((dynamic) command);
        }
        
        public async Task<T> DispatchAsync<T>(IQuery<T> query)
        {
            var type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            T result = await handler.ExecuteAsync((dynamic) query);

            return result;
        }
    }
}