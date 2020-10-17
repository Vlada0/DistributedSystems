using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Config;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
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

        public async Task<IEnumerable<TEntity>> ReadCollectionAsync(string query)
        {
            var entities = await _mapper.FetchAsync<TEntity>(query);

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