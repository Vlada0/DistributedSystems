using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Infrastructure.Config;
using AviaSalesApi.Infrastructure.Exceptions;
using Cassandra;
using Cassandra.Mapping;

namespace AviaSalesApi.Data.Repository.Impl
{
    public class CassandraRepository<TEntity> : ICassandraRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMapper _mapper;
        
        public CassandraRepository(ICassandraDbConfig cfg)
        {
            var cluster = Cluster.Builder().AddContactPoint(cfg.Host).Build();
            var session = cluster.Connect(cfg.KeySpace);
            _mapper = new Mapper(session);
        }

        public async Task<IEnumerable<TEntity>> ReadCollectionAsync(string query, params object[] args)
        {
            string md = args[0] as string;
            var entities = await _mapper.FetchAsync<TEntity>("FROM ticket_by_place_from_place_to_takeoff_day WHERE country_from = ? AND city_from = ? AND country_to = ? AND city_to = ? AND takeoff_day = ?", md, "Chisinau", "Australia", "Sydney", new DateTime(2020, 12, 17));

            return entities;
        }

        public async Task<TEntity> ReadOneAsync(string query)
        {
            var entity = await _mapper.FirstOrDefaultAsync<TEntity>(query);
            if (entity == null)
            {
                throw EntityNotFoundException.OfType<TEntity>();
            }

            return entity;
        }

        public async Task InsertAsync(TEntity entity) => await _mapper.InsertAsync(entity);

        public async Task DeleteAsync(TEntity entity, string query)
        {
            var entityToDelete = await _mapper.FirstOrDefaultAsync<TEntity>(query);
            
            if (entityToDelete == null)
            {
                throw EntityNotFoundException.OfType<TEntity>();
            }

            await _mapper.DeleteAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity, string query)
        {
            var entityToUpdate = await _mapper.FirstOrDefaultAsync<TEntity>(query);
            
            if (entityToUpdate == null)
            {
                throw EntityNotFoundException.OfType<TEntity>();
            }

            await _mapper.UpdateAsync(entity);
        }
    }
}