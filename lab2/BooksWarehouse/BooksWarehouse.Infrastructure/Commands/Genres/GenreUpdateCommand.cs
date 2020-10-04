using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Infrastructure.Commands.Genres
{
    public class GenreUpdateCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        internal sealed class GenreUpdateCommandHandler : ICommandHandler<GenreUpdateCommand>
        {
            private readonly IMongoDbRepository<Genre> _genres;
            private readonly IMapper _mapper;

            public GenreUpdateCommandHandler(IMongoDbRepository<Genre> genres, IMapper mapper)
            {
                _genres = genres;
                _mapper = mapper;
            }

            public async Task ExecuteAsync(GenreUpdateCommand command)
            {
                var genre = _mapper.Map<Genre>(command);
                await _genres.ReplaceOneAsync(genre);
            }
        }
    }
}