using System;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Genres
{
    public class GenreDeleteByNameCommand : ICommand
    {
        public string Name { get; set; }

        internal sealed class GenreDeleteByNameCommandHandler : ICommandHandler<GenreDeleteByNameCommand>
        {
            private readonly IMongoDbRepository<Genre> _genres;

            public GenreDeleteByNameCommandHandler(IMongoDbRepository<Genre> genres) =>_genres = genres;

            public async Task ExecuteAsync(GenreDeleteByNameCommand command) =>
                await _genres.DeleteOneAsync(g => g.Name.ToLower() == command.Name.ToLower());
        }
    }
}