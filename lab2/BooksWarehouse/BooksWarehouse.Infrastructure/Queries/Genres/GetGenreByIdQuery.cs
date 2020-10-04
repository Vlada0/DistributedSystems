using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Genres;

namespace BooksWarehouse.Infrastructure.Queries.Genres
{
    public class GetGenreByIdQuery : IQuery<GenreModel>
    {
        private readonly Guid _id;

        public GetGenreByIdQuery(Guid id)
        {
            _id = id;
        }
        
        internal sealed class GetGenreByIdQueryHandler : IQueryHandler<GetGenreByIdQuery, GenreModel>
        {
            private readonly IMongoDbRepository<Genre> _genres;
            private readonly IMapper _mapper;

            public GetGenreByIdQueryHandler(IMongoDbRepository<Genre> genres, IMapper mapper)
            {
                _genres = genres;
                _mapper = mapper;
            }

            public async Task<GenreModel> ExecuteAsync(GetGenreByIdQuery query)
            {
                var genre = await _genres.FindByIdAsync(query._id);
                return _mapper.Map<GenreModel>(genre);
            }
        }
    }
}