using System;
using System.Collections.Generic;

namespace BooksWarehouse.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}