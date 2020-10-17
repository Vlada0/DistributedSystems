using System;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Helpers;

namespace AviaSalesApi.Data
{
    public static class Seed
    {
        public static async Task SeedDataAsync(
            ICassandraRepository<Ticket> ticketRepository, 
            ICassandraRepository<TicketById> ticketByIdRepository,
            IJsonFileProcessor processor)
        {
            var tickets = await processor.ProcessFileAsync(
                @"D:\Универ\4 курс\1 семестр\PAD\LABS\DistributedSystems\lab2\AviaSalesApi\AviaSalesApi\seed.json");

            foreach (var ticket in tickets)
            {
                ticket.Id = Guid.NewGuid();
                var ticketById = TicketById.From(ticket);

                await ticketRepository.InsertAsync(ticket);
                await ticketByIdRepository.InsertAsync(ticketById);
            }
        }
    }
}