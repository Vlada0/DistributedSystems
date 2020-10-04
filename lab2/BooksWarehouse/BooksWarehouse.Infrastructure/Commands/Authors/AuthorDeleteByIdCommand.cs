using System;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Authors
{
    public class AuthorDeleteByIdCommand : ICommand
    {
        private readonly Guid _id;

        public AuthorDeleteByIdCommand(Guid id)
        {
            _id = id;
        }
        
        internal sealed class AuthorDeleteByIdCommandHandler : ICommandHandler<AuthorDeleteByIdCommand>
        {
            private readonly IMongoDbRepository<Author> _authors;

            public AuthorDeleteByIdCommandHandler(IMongoDbRepository<Author> authors)
            {
                _authors = authors;
            }

            public async Task ExecuteAsync(AuthorDeleteByIdCommand command) => 
                await _authors.DeleteByIdAsync(command._id);
        }
    }
}