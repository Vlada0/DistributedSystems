using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Infrastructure;
using AviaSalesApi.Models.Tickets;
using AviaSalesApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AviaSalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketModel>>> TicketsGet([FromQuery] TicketQueryParameters query)
        {
            return Ok(await _ticketService.GetFilteredTicketsAsync(query));
        }

        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketModel>> TicketGetById([FromRoute] Guid ticketId)
        {
            return Ok(await _ticketService.GetTicketById(ticketId));
        }

        [HttpPost]
        public async Task<ActionResult<TicketModel>> TicketCreate([FromBody] TicketCreateUpdateModel updateModel)
        {
            var ticket = await _ticketService.AddTicketAsync(updateModel);
            return CreatedAtAction(nameof(TicketGetById), new {ticketId = ticket.Id}, ticket);
        }

        [HttpPut("{ticketId}")]
        public async Task<IActionResult> TicketUpdate([FromRoute] Guid ticketId,
            [FromBody] TicketCreateUpdateModel model)
        {
            await _ticketService.UpdateTicketAsync(ticketId, model);
            return NoContent();
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> TicketDelete([FromRoute] Guid ticketId)
        {
            await _ticketService.DeleteTicketAsync(ticketId);
            return NoContent();
        }
    }
}