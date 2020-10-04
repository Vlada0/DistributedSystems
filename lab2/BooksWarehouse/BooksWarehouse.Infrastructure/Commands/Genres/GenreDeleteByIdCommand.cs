using System;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Genres
{
    public class GenreDeleteByIdCommand : ICommand
    {
        private readonly Guid _id;

        public GenreDeleteByIdCommand(Guid id)
        {
            _id = id;
        }
        
        internal sealed class GenreDeleteByIdCommandHandler : ICommandHandler<GenreDeleteByIdCommand>
        {
            private readonly IMongoDbRepository<Genre> _genres;

            public GenreDeleteByIdCommandHandler(IMongoDbRepository<Genre> genres) =>_genres = genres;

            public async Task ExecuteAsync(GenreDeleteByIdCommand command) => 
                await _genres.DeleteByIdAsync(command._id);
        }
    }
}