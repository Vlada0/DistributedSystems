using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Genres;

namespace BooksWarehouse.Infrastructure.Queries.Genres
{
    public class GetGenresQuery : IQuery<IEnumerable<GenreModel>>
    {
        internal sealed class GetGenresQueryHandler : IQueryHandler<GetGenresQuery, IEnumerable<GenreModel>>
        {
            private readonly IMongoDbRepository<Genre> _genres;

            public GetGenresQueryHandler(IMongoDbRepository<Genre> genres)
            {
                _genres = genres;
            }

            public async Task<IEnumerable<GenreModel>> ExecuteAsync(GetGenresQuery query)
            {
                var genres = (await _genres.GetListAsync())
                    .Select(g => new GenreModel
                    {
                        GenreId = g.Id,
                        Name = g.Name
                    });

                return genres;
            }
        }
    }
}