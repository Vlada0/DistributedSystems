using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable ClassNeverInstantiated.Global

namespace BooksWarehouse.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        [BsonRequired] public Guid CountryId { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}