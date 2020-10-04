using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            private readonly IMapper _mapper;

            public GetAuthorsQueryHandler(IMongoDbRepository<Author> authors, IMapper mapper)
            {
                _authors = authors;
                _mapper = mapper;
            }

            public async Task<IEnumerable<AuthorModel>> ExecuteAsync(GetAuthorsQuery query)
            {
                var authors = await _authors.GetListAsync();
                return _mapper.Map<IEnumerable<AuthorModel>>(authors);
            }
        }
    }
}