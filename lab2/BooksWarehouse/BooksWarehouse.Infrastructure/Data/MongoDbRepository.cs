using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BooksWarehouse.Core.Exceptions;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Config;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BooksWarehouse.Infrastructure.Data
{
    public class MongoDbMongoDbRepository<T> : IMongoDbRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbMongoDbRepository(IMongoDbConnectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);
            _collection = database.GetCollection<T>($"{typeof(T).Name}s");
        }

        public IQueryable<T> AsQueryable() => _collection.AsQueryable();

        public async Task<IEnumerable<T>> GetFilteredByAsync(Expression<Func<T, bool>> expression) =>
            await _collection.AsQueryable().Where(expression).ToListAsync();

        public async Task<IEnumerable<T>> GetListAsync() => await _collection.AsQueryable().ToListAsync();

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> expression)
        {
            var document = await _collection.AsQueryable().FirstOrDefaultAsync(expression);
            if (document == null)
            {
                throw new EntityNotFoundException($"{typeof(T).ToString().Split('.').Last()} not found.");
            }
            return document;
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(d => d.Id, id);
            var document = await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
            if (document == null)
            {
                throw EntityNotFoundException.OfType<T>(id);
            }
            return document;
        }

        public async Task<Guid> InsertOneAsync(T document)
        {
            if (document == null)
            {
                throw new BadRequestException($"Document {typeof(T).ToString().Split('.').Last()} was not provided.");
            }
            
            document.Id = Guid.NewGuid();

            await _collection.InsertOneAsync(document);

            return document.Id;
        }

        public async Task InsertManyAsync(ICollection<T> documents)
        {
            if (documents == null)
            {
                throw new BadRequestException($"Document {typeof(T).ToString().Split('.').Last()}s were not provided.");
            }

            await _collection.InsertManyAsync(documents);
        }

        public async Task ReplaceOneAsync(T document)
        {
            if (document == null)
            {
                throw new BadRequestException($"Document {typeof(T).ToString().Split('.').Last()} was not provided.");
            }

            var filter = Builders<T>.Filter.Eq(d => d.Id, document.Id);
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task DeleteOneAsync(Expression<Func<T, bool>> expression)
        {
            var document = await _collection.AsQueryable().FirstOrDefaultAsync(expression);
            if (document == null)
            {
                throw new EntityNotFoundException($"{typeof(T).ToString().Split('.').Last()} not found.");
            }

            await _collection.FindOneAndDeleteAsync(expression);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(d => d.Id, id);
            var document = await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
            if (document == null)
            {
                throw EntityNotFoundException.OfType<T>(id);
            }

            await _collection.FindOneAndDeleteAsync(filter);
        }
    }
}