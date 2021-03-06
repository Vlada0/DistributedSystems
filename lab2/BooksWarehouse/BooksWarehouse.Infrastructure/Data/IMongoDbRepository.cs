﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;

namespace BooksWarehouse.Infrastructure.Data
{
    public interface IMongoDbRepository<T> where T : BaseEntity
    {
        IQueryable<T> AsQueryable();
        Task<IEnumerable<T>> GetFilteredByAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetListAsync();
        Task<T> FindOneAsync(Expression<Func<T, bool>> expression);
        Task<T> FindByIdAsync(Guid id);
        Task<Guid> InsertOneAsync(T document);
        Task InsertManyAsync(ICollection<T> documents);
        Task ReplaceOneAsync(T document);
        Task DeleteOneAsync(Expression<Func<T, bool>> expression);
        Task DeleteByIdAsync(Guid id);

    }
}