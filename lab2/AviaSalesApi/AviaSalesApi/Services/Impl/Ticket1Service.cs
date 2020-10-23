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
    public class Ticket1Service : ITicketService
    {
        private readonly Cassandra.Mapping.IMapper _dbMapper;

        public Ticket1Service(ICassandraDbConfig cfg)
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
                $"AND takeoff_year = {query.TakeOffDay?.Year} AND takeoff_month = {query.TakeOffDay?.Month} " + 
                $"AND takeoff_day = {query.TakeOffDay?.Day};";
            
            cqlQuery = cqlQuery.Replace('\"', '\'');
            
            var tickets = await _dbMapper.FetchAsync<Ticket1>(cqlQuery);

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

        public async Task<TicketModel> AddTicketAsync(TicketCreateUpdateModel updateModel)
        {
            var ticket = Ticket1.From(updateModel);
            ticket.Id = Guid.NewGuid();
            
            await _dbMapper.InsertAsync(ticket);
            await _dbMapper.InsertAsync(TicketById.From(ticket));

            return TicketModel.From(ticket);
        }

        public async Task UpdateTicketAsync(Guid ticketId, TicketCreateUpdateModel model)
        {
            var ticketById = await _dbMapper.SingleOrDefaultAsync<TicketById>("WHERE id = ?", ticketId);
            if (ticketById == null)
            {
                throw EntityNotFoundException.OfType<TicketById>();
            }
            
            await _dbMapper.UpdateAsync(ticketById);
            
            var cqlQuery =
                $"DELETE FROM ticket_by_place_from_place_to_takeoff_day " +
                $"WHERE country_from = '{ticketById.CountryFrom}' AND city_from = '{ticketById.CityFrom}' " +
                $"AND country_to = '{ticketById.CountryTo}' AND city_to = '{ticketById.CityTo}' " +
                $"AND takeoff_year = {ticketById.TakeOffYear} AND takeoff_month = {ticketById.TakeOffMonth} " +
                $"AND takeoff_day = {ticketById.TakeOffDay} AND id = {ticketById.Id}";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            await _dbMapper.ExecuteAsync(cqlQuery);
            var newTicket = Ticket1.From(ticketById);
            await _dbMapper.InsertAsync(newTicket);
        }

        public async Task DeleteTicketAsync(Guid ticketId)
        {
            var ticketById = await _dbMapper.SingleOrDefaultAsync<TicketById>("WHERE id = ?", ticketId);
            if (ticketById == null)
            {
                throw EntityNotFoundException.OfType<TicketById>();
            }
            var ticket = Ticket1.From(ticketById);

            await _dbMapper.DeleteAsync(ticketById);

            var cqlQuery =
                $"DELETE FROM ticket_by_place_from_place_to_takeoff_day " +
                $"WHERE country_from = '{ticketById.CountryFrom}' AND city_from = '{ticketById.CityFrom}' " +
                $"AND country_to = '{ticketById.CountryTo}' AND city_to = '{ticketById.CityTo}' " +
                $"AND takeoff_year = {ticketById.TakeOffYear} AND takeoff_month = {ticketById.TakeOffMonth} " +
                $"AND takeoff_day = {ticketById.TakeOffDay} AND id = {ticketById.Id}";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            await _dbMapper.ExecuteAsync(cqlQuery);
        }
    }
}