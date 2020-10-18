using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;

namespace AviaSalesApi.Data.Repository.Interfaces
{
    public interface ICassandraRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> ReadCollectionAsync(string query, params object[] args);
        Task<TEntity> ReadOneAsync(string query);
        Task InsertAsync(TEntity entity);
        Task DeleteAsync(TEntity entity, string query);
        Task UpdateAsync(TEntity entity, string query);
    }
}