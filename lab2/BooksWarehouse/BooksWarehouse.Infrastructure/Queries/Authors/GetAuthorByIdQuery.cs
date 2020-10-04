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

            public GetAuthorByIdQueryHandler(IMongoDbRepository<Author> authors)
            {
                _authors = authors;
            }

            public async Task<AuthorModel> ExecuteAsync(GetAuthorByIdQuery query)
            {
                var author = await _authors.FindByIdAsync(query._id);

                var authorModel = new AuthorModel
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    BirthDate = author.BirthDate,
                    DateOfDeath = author.DateOfDeath,
                    Country = author.Country
                };

                return authorModel;
            }
        }
    }
}