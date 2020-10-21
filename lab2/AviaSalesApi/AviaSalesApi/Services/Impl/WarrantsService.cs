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
    public class WarrantsService : IWarrantsService
    {
        private readonly IMapper _mapper;
        private readonly Cassandra.Mapping.IMapper _dbMapper;

        public WarrantsService(IMapper mapper, ICassandraDbConfig cfg)
        {
            var cluster = Cluster.Builder().AddContactPoint(cfg.Host).Build();
            var session = cluster.Connect(cfg.KeySpace);
            _dbMapper = new Cassandra.Mapping.Mapper(session);
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarrantModel>> GetWarrantsByIban(string iban)
        {
            var cqlQuery = $"FROM warrants_by_passenger_iban WHERE passenger_iban = {iban};";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            var warrants = await _dbMapper.FetchAsync<WarrantByPassengerIban>(cqlQuery);
            var models = _mapper.Map<IEnumerable<WarrantModel>>(warrants);
            
            return models;
        }

        public async Task<WarrantModel> GetWarrantByIbanAndId(string iban, Guid id)
        {
            var cqlQuery = $"FROM warrants_by_passenger_iban WHERE passenger_iban = {iban} AND id = {id};";
            cqlQuery = cqlQuery.Replace('\"', '\'');

            var warrant = await _dbMapper.SingleOrDefaultAsync<WarrantByPassengerIbanAndTicketId>(cqlQuery);
            if (warrant == null)
            {
                throw EntityNotFoundException.OfType<WarrantByPassengerIbanAndTicketId>();
            }

            var model = _mapper.Map<WarrantModel>(warrant);
            return model;
        }

        public async Task<WarrantModel> CreateWarrant(WarrantCreateUpdateModel model)
        {
            var warrantByIban = _mapper.Map<WarrantByPassengerIban>(model);
            warrantByIban.Id = Guid.NewGuid();
            await _dbMapper.InsertAsync(warrantByIban);

            var warrantById = _mapper.Map<WarrantByPassengerIbanAndTicketId>(model);
            warrantById.Id = warrantByIban.Id;
            await _dbMapper.InsertAsync(warrantById);

            return _mapper.Map<WarrantModel>(warrantByIban);
        }
    }
}