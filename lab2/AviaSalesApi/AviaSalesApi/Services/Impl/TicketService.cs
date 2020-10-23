using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Infrastructure;
using AviaSalesApi.Models.Tickets;
using AviaSalesApi.Services.Interfaces;

namespace AviaSalesApi.Services.Impl
{
    public class TicketService : ITicketService
    {
        private readonly IMongoRepository<Ticket> _mongoRepository;

        public TicketService(IMongoRepository<Ticket> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<IEnumerable<TicketModel>> GetFilteredTicketsAsync(TicketQueryParameters query)
        {
            var properties = query.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var pe = Expression.Parameter(typeof(Ticket), "t");
            var expressionBody =
                (from property in properties
                    where property.GetValue(query, null) != null
                    let member = Expression.Property(pe, property.Name)
                    let right = Expression.Constant(property.GetValue(query, null))
                    select Expression.Equal(member, right)).Aggregate<Expression, Expression>(null,
                    (current, body) => current == null ? body : 
                        Expression.AndAlso(current, body));

            var expression = Expression.Lambda<Func<Ticket, bool>>(expressionBody, pe);

            var tickets = await _mongoRepository.FilterByAsync(expression);
            var models = tickets.Select(TicketModel.From);

            return models;
        }

        public async Task<TicketModel> GetTicketById(Guid id)
        {
            var ticket = await _mongoRepository.FindByIdAsync(id);
            return TicketModel.From(ticket);
        }

        public async Task<TicketModel> AddTicketAsync(TicketCreateUpdateModel updateModel)
        {
            var ticket = Ticket.From(updateModel);
            var id = await _mongoRepository.InsertOneAsync(ticket);

            var model = TicketModel.From(ticket);

            return model;
        }

        public async Task UpdateTicketAsync(Guid ticketId, TicketCreateUpdateModel model)
        {
            var ticket = Ticket.From(model);
            ticket.Id = ticketId;

            await _mongoRepository.ReplaceOneAsync(ticket);
        }

        public async Task DeleteTicketAsync(Guid ticketId)
        {
            await _mongoRepository.DeleteByIdAsync(ticketId);
        }
    }
}