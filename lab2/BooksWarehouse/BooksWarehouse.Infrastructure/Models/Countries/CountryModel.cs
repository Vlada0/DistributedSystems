using System;

namespace BooksWarehouse.Infrastructure.Models.Countries
{
    public class CountryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}