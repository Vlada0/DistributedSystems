using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Infrastructure;
using AviaSalesApi.Models.Tickets;

namespace AviaSalesApi.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketModel>> GetFilteredTicketsAsync(TicketQueryParameters query);
        Task<TicketModel> GetTicketById(Guid id);
        Task<Guid> AddTicketAsync(TicketCreateModel model);
        Task DeleteTicketAsync(Guid ticketId);
    }
}