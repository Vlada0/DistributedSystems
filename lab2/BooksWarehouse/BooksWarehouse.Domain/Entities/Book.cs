using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable ClassNeverInstantiated.Global

namespace BooksWarehouse.Domain.Entities
{
	public class Book : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Language { get; set; }
		[BsonRequired] public Guid AuthorId { get; set; }
		[BsonRequired] public Guid GenreId { get; set; }
	}
}
