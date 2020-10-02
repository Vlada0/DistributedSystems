using System;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Core.Exceptions;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Countries;

namespace BooksWarehouse.Infrastructure.Queries.Countries
{
    public class GetCountryByCodeQuery : IQuery<CountryModel>
    {
        public string Code { get; private set; }

        public GetCountryByCodeQuery(string code)
        {
            Code = code;
        }

        internal sealed class GetCountryByCodeQueryHandler : IQueryHandler<GetCountryByCodeQuery, CountryModel>
        {
            private readonly IMongoDbRepository<Country> _countries;
            private readonly IMapper _mapper;

            public GetCountryByCodeQueryHandler(IMongoDbRepository<Country> countries, IMapper mapper)
            {
                _countries = countries;
                _mapper = mapper;
            }

            public async Task<CountryModel> ExecuteAsync(GetCountryByCodeQuery query)
            {
                if (string.IsNullOrWhiteSpace(query.Code))
                {
                    throw new BadRequestException($"Country code must be specified.");
                }
                
                var country =
                    await _countries.FindOneAsync(c => string.Equals(c.Code, query.Code));
                
                return _mapper.Map<CountryModel>(country);
            }
        }
    }
}