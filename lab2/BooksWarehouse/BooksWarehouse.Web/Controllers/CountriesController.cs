using System.Collections.Generic;
using System.Threading.Tasks;
using BooksWarehouse.Infrastructure;
using BooksWarehouse.Infrastructure.Models.Countries;
using BooksWarehouse.Infrastructure.Queries.Countries;
using Microsoft.AspNetCore.Mvc;

namespace BooksWarehouse.Web.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : BaseController
    {
        public CountriesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryModel>>> CountriesGet()
        {
            var countries = await _mediator.DispatchAsync(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<CountryModel>> CountryGetByCode([FromRoute] string code)
        {
            var country = await _mediator.DispatchAsync(new GetCountryByCodeQuery(code));
            return Ok(country);
        }
    }
}