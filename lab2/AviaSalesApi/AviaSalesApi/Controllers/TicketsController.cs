using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Infrastructure;
using AviaSalesApi.Models.Tickets;
using AviaSalesApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<TicketModel>> TicketGetById(Guid ticketId)
        {
            var ticket = await _ticketService.GetTicketById(ticketId);
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> TicketCreate([FromBody] TicketCreateModel model)
        {
            var id = await _ticketService.AddTicketAsync(model);
            return CreatedAtAction(nameof(TicketGetById), new {ticketId = id});
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> TicketDelete(Guid ticketId)
        {
            await _ticketService.DeleteTicketAsync(ticketId);
            return NoContent();
        }
    }
}