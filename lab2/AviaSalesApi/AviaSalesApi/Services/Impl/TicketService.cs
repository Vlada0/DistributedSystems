using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Infrastructure;
using AviaSalesApi.Infrastructure.Config;
using AviaSalesApi.Infrastructure.Exceptions;
using AviaSalesApi.Models.Tickets;
using AviaSalesApi.Services.Interfaces;
using Cassandra;

namespace AviaSalesApi.Services.Impl
{
    public class TicketService : ITicketService
    {
        private readonly Cassandra.Mapping.IMapper _dbMapper;

        public TicketService(ICassandraDbConfig cfg)
        {
            var cluster = Cluster.Builder().AddContactPoint(cfg.Host).Build();
            var session = cluster.Connect(cfg.KeySpace);
            _dbMapper = new Cassandra.Mapping.Mapper(session);
        }

        public async Task<IEnumerable<TicketModel>> GetFilteredTicketsAsync(TicketQueryParameters query)
        {
            var cqlQuery = 
                $"FROM ticket_by_place_from_place_to_takeoff_day " + 
                $"WHERE country_from = {query.CountryFrom} AND city_from = {query.CityFrom} " + 
                $"AND country_to = {query.CountryTo} AND city_to = {query.CityTo} " + 
                $"AND takeoff_year = {query.TakeOffDay.Year} AND takeoff_month = {query.TakeOffDay.Month} " + 
                $"AND takeoff_day = {query.TakeOffDay.Day};";
            
            cqlQuery = cqlQuery.Replace('\"', '\'');
            
            var tickets = await _dbMapper.FetchAsync<Ticket>(cqlQuery);

            return tickets.Select(TicketModel.From);
        }

        public async Task<TicketModel> GetTicketById(Guid id)
        {
            var ticket = await _dbMapper.SingleOrDefaultAsync<TicketById>("WHERE id = ?", id);
            
            if (ticket == null)
            {
                throw EntityNotFoundException.OfType<TicketById>();
            }
            return TicketModel.From(ticket);
        }

        public async Task<Guid> AddTicketAsync(TicketCreateModel model)
        {
            var ticket = Ticket.From(model);
            ticket.Id = Guid.NewGuid();

            await _dbMapper.InsertAsync(ticket);
            await _dbMapper.InsertAsync(TicketById.From(ticket));

            return ticket.Id;
        }

        public async Task DeleteTicketAsync(Guid ticketId)
        {
            var ticketById = await _dbMapper.SingleOrDefaultAsync<TicketById>("WHERE id = ?", ticketId);
            if (ticketById == null)
            {
                throw EntityNotFoundException.OfType<TicketById>();
            }
            var ticket = Ticket.From(ticketById);

            await _dbMapper.DeleteAsync(ticketById);

            var cqlQuery =
                $"DELETE FROM ticket_by_place_from_place_to_takeoff_day" +
                $"WHERE country_from = {ticket.CountryFrom} AND city_from = {ticket.CityFrom}" +
                $"AND country_to = {ticket.CountryTo} AND city_to = {ticket.CityTo}" +
                $"AND takeoff_year = {ticket.TakeOffYear} AND takeoff_month = {ticket.TakeOffMonth} " +
                $"AND takeoff_day = {ticket.TakeOffDay} AND id = {ticket.Id}";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            await _dbMapper.ExecuteAsync(cqlQuery);
        }
    }
}