using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Authors;

namespace BooksWarehouse.Infrastructure.Queries.Authors
{
    public sealed class GetAuthorsQuery : IQuery<IEnumerable<AuthorModel>>
    {
        internal  sealed class GetAuthorsQueryHandler : IQueryHandler<GetAuthorsQuery, IEnumerable<AuthorModel>>
        {
            private readonly IMongoDbRepository<Author> _authors;
            private readonly IMongoDbRepository<Country> _countries;

            public GetAuthorsQueryHandler(IMongoDbRepository<Author> authors, IMongoDbRepository<Country> countries)
            {
                _authors = authors;
                _countries = countries;
            }

            public async Task<IEnumerable<AuthorModel>> ExecuteAsync(GetAuthorsQuery query)
            {
                var authors = await _authors.GetListAsync();
                var result =
                    (from country in (await _countries.GetListAsync()).Where(c =>
                            authors.Select(a => a.CountryId).Contains(c.Id))
                        join author in authors on country.Id equals author.CountryId
                        select new AuthorModel
                        {
                            Id = author.Id,
                            FirstName = author.FirstName,
                            LastName = author.LastName,
                            BirthDate = author.BirthDate,
                            DateOfDeath = author.DateOfDeath,
                            Country = country.Name,
                            CountryCode = country.Code
                        }).ToList();

                return result;
            }
        }
    }
}