using System;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Authors;

namespace BooksWarehouse.Infrastructure.Queries.Authors
{
    public class GetAuthorByIdQuery : IQuery<AuthorModel>
    {
        private readonly Guid _id;

        public GetAuthorByIdQuery(Guid id)
        {
            _id = id;
        }
        
        internal sealed class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, AuthorModel>
        {
            private readonly IMongoDbRepository<Author> _authors;
            private readonly IMongoDbRepository<Country> _countries;

            public GetAuthorByIdQueryHandler(IMongoDbRepository<Author> authors, IMongoDbRepository<Country> countries)
            {
                _authors = authors;
                _countries = countries;
            }

            public async Task<AuthorModel> ExecuteAsync(GetAuthorByIdQuery query)
            {
                var author = await _authors.FindByIdAsync(query._id);
                var country = await _countries.FindOneAsync(c => c.Id == author.CountryId);

                var authorModel = new AuthorModel
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    BirthDate = author.BirthDate,
                    DateOfDeath = author.DateOfDeath,
                    Country = country.Name,
                    CountryCode = country.Code
                };

                return authorModel;
            }
        }
    }
}