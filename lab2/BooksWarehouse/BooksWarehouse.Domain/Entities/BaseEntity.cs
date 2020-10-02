using System;
using MongoDB.Bson.Serialization.Attributes;

namespace BooksWarehouse.Domain.Entities
{
    public abstract class BaseEntity
    {
        [BsonId] public Guid Id { get; set; }
    }
}