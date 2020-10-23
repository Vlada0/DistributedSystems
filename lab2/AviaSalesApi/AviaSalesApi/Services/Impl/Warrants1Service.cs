using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Infrastructure.Config;
using AviaSalesApi.Infrastructure.Exceptions;
using AviaSalesApi.Models.Warrants;
using AviaSalesApi.Services.Interfaces;
using Cassandra;
using Cassandra.Mapping;
using IMapper = AutoMapper.IMapper;

namespace AviaSalesApi.Services.Impl
{
    public class Warrants1Service : IWarrantsService
    {
        private readonly IMapper _mapper;
        private readonly Cassandra.Mapping.IMapper _dbMapper;

        public Warrants1Service(IMapper mapper, ICassandraDbConfig cfg)
        {
            var cluster = Cluster.Builder().AddContactPoint(cfg.Host).Build();
            var session = cluster.Connect(cfg.KeySpace);
            _dbMapper = new Cassandra.Mapping.Mapper(session);
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarrantModel>> GetWarrantsByIbanAsync(string iban)
        {
            var cqlQuery = $"FROM warrants_by_passenger_iban WHERE passenger_iban = {iban};";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            var warrants = await _dbMapper.FetchAsync<WarrantByPassengerIban>(cqlQuery);
            var models = _mapper.Map<IEnumerable<WarrantModel>>(warrants);
            
            return models;
        }

        public async Task<WarrantModel> GetWarrantByIbanAndIdAsync(string iban, Guid id)
        {
            var cqlQuery = $"FROM warrants_by_passenger_iban WHERE passenger_iban = \"{iban}\" AND id = {id};";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            var warrant = await _dbMapper.SingleOrDefaultAsync<WarrantByPassengerIbanAndTicketId>(cqlQuery);
            if (warrant == null)
            {
                throw EntityNotFoundException.OfType<WarrantByPassengerIbanAndTicketId>();
            }

            var model = _mapper.Map<WarrantModel>(warrant);
            return model;
        }

        public async Task<WarrantModel> CreateWarrantAsync(WarrantCreateUpdateModel model)
        {
            var warrantByIban = _mapper.Map<WarrantByPassengerIban>(model);
            warrantByIban.Id = Guid.NewGuid();
            await _dbMapper.InsertAsync(warrantByIban);

            var warrantById = _mapper.Map<WarrantByPassengerIbanAndTicketId>(model);
            warrantById.Id = warrantByIban.Id;
            await _dbMapper.InsertAsync(warrantById);

            return _mapper.Map<WarrantModel>(warrantByIban);
        }

        public async Task UpdateWarrantAsync(string iban, Guid warrantId, WarrantCreateUpdateModel model)
        {
            var warrantById = _mapper.Map<WarrantByPassengerIban>(model);
            warrantById.Id = warrantId;
            await _dbMapper.UpdateAsync<WarrantByPassengerIban>(warrantById);

            var warrantByTicketIdAndIban = _mapper.Map<WarrantByPassengerIbanAndTicketId>(model);
            warrantByTicketIdAndIban.Id = warrantId;
            await _dbMapper.UpdateAsync<WarrantByPassengerIbanAndTicketId>(warrantByTicketIdAndIban);
        }

        public async Task DeleteWarrantAsync(string iban, Guid warrantId)
        {
            var cqlQuery = $"FROM warrants_by_passenger_iban WHERE passenger_iban = \"{iban}\" AND id = {warrantId};";
            cqlQuery = cqlQuery.Replace('\"', '\'');
            var warrant = await _dbMapper.SingleOrDefaultAsync<WarrantByPassengerIbanAndTicketId>(cqlQuery);
            
            cqlQuery = $"DELETE FROM warrants_by_passenger_iban WHERE passenger_iban = \"{iban}\" AND id = {warrantId};";
            cqlQuery = cqlQuery.Replace('\"', '\'');
            await _dbMapper.ExecuteAsync(cqlQuery);

            cqlQuery =
                $"DELETE FROM warrant_by_passenger_iban_and_ticket_id WHERE passenger_iban = \"{iban}\" AND ticket_id = {warrant.TicketId} AND id = {warrantId};";
            cqlQuery = cqlQuery.Replace('\"', '\'');
            await _dbMapper.ExecuteAsync(cqlQuery);
        }
    }
}