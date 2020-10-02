using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure;
using BooksWarehouse.Infrastructure.Commands.Authors;
using BooksWarehouse.Infrastructure.Models.Authors;
using BooksWarehouse.Infrastructure.Queries.Authors;
using Microsoft.AspNetCore.Mvc;

namespace BooksWarehouse.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : BaseController
    {
        public AuthorsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> AuthorsGet()
        {
            var authors = await _mediator.DispatchAsync(new GetAuthorsQuery());
            
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorModel>> AuthorGetById([FromRoute] Guid authorId)
        {
            var author = await _mediator.DispatchAsync(new GetAuthorByIdQuery(authorId));
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorModel>> AuthorCreate([FromBody] AuthorCreateCommand command)
        {
            await _mediator.DispatchAsync(command);
            var newAuthor = await _mediator.DispatchAsync(new GetAuthorByIdQuery(command.Id));

            return CreatedAtAction(nameof(AuthorGetById), new {authorId = newAuthor.Id}, newAuthor);
        }

        [HttpPut("{authorId}")]
        public async Task<IActionResult> AuthorUpdate([FromRoute] Guid authorId, [FromBody] AuthorUpdateCommand command)
        {
            command.Id = authorId;
            await _mediator.DispatchAsync(command);

            return NoContent();
        }
    }
}