using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApiCopyOne.Infrastructure;
using AviaSalesApiCopyOne.Models.Tickets;

namespace AviaSalesApiCopyOne.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketModel>> GetFilteredTicketsAsync(TicketQueryParameters query);
        Task<TicketModel> GetTicketById(Guid id);
        Task<TicketModel> AddTicketAsync(TicketCreateUpdateModel updateModel);
        Task UpdateTicketAsync(Guid ticketId, TicketCreateUpdateModel model);
        Task DeleteTicketAsync(Guid ticketId);
    }
}