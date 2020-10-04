using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BooksWarehouse.Infrastructure;
using BooksWarehouse.Infrastructure.Commands.Genres;
using BooksWarehouse.Infrastructure.Models.Genres;
using BooksWarehouse.Infrastructure.Queries.Genres;
using Microsoft.AspNetCore.Mvc;

namespace BooksWarehouse.Web.Controllers
{
    [Route("api/[controller]")]
    public class GenresController : BaseController
    {
        public GenresController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GenresGet()
        {
            var genres = await _mediator.DispatchAsync(new GetGenresQuery());
            return Ok(genres);
        }
        
        [HttpGet("{genreId}")]
        public async Task<ActionResult<IEnumerable<GenreModel>>> GenreGetById([FromRoute] Guid genreId)
        {
            var genre = await _mediator.DispatchAsync(new GetGenreByIdQuery(genreId));
            return Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult<GenreModel>> GenreCreate([FromBody] GenreCreateCommand command)
        {
            await _mediator.DispatchAsync(command);
            var genre = await _mediator.DispatchAsync(new GetGenreByIdQuery(command.Id));
            return CreatedAtAction(nameof(GenreGetById), new {genreId = command.Id}, genre);
        }
        
        [HttpPut]
        public async Task<IActionResult> GenreUpdate([FromBody] GenreUpdateCommand command)
        {
            await _mediator.DispatchAsync(command);
            return NoContent();
        }

        [HttpDelete("{genreId}")]
        public async Task<IActionResult> GenreDeleteById([FromRoute] Guid genreId)
        {
            await _mediator.DispatchAsync(new GenreDeleteByIdCommand(genreId));
            return NoContent();
        }
        
        [HttpDelete]
        public async Task<IActionResult> GenreDeleteByName([FromQuery] GenreDeleteByNameCommand command)
        {
            await _mediator.DispatchAsync(command);
            return NoContent();
        }
    }
}