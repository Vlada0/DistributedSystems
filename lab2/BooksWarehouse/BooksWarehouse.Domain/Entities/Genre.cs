using System.Collections.Generic;

namespace BooksWarehouse.Domain.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}