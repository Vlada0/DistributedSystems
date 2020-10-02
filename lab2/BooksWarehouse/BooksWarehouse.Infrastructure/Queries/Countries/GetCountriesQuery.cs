using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;
using BooksWarehouse.Infrastructure.Models.Countries;

namespace BooksWarehouse.Infrastructure.Queries.Countries
{
    public class GetCountriesQuery : IQuery<IEnumerable<CountryModel>>
    {
        internal sealed class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, IEnumerable<CountryModel>>
        {
            private readonly IMongoDbRepository<Country> _countries;
            private readonly IMapper _mapper;

            public GetCountriesQueryHandler(IMongoDbRepository<Country> countries, IMapper mapper)
            {
                _countries = countries;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CountryModel>> ExecuteAsync(GetCountriesQuery query)
            {
                var countries = await _countries.GetListAsync();
                
                return _mapper.Map<IEnumerable<CountryModel>>(countries);
            }
        }
    }
}