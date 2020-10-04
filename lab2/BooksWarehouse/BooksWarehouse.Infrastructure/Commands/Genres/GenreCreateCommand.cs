using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Genres
{
    public class GenreCreateCommand : ICommand
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        
        internal sealed class GenreCreateCommandHandler : ICommandHandler<GenreCreateCommand>
        {
            private readonly IMongoDbRepository<Genre> _genres;
            private readonly IMapper _mapper;

            public GenreCreateCommandHandler(IMongoDbRepository<Genre> genres, IMapper mapper)
            {
                _genres = genres;
                _mapper = mapper;
            }

            public async Task ExecuteAsync(GenreCreateCommand command)
            {
                var genre = _mapper.Map<Genre>(command);
                command.Id = await _genres.InsertOneAsync(genre);
            }
        }
    }
}