using System;
using BooksWarehouse.Domain.Entities;

namespace BooksWarehouse.Infrastructure.Models.Authors
{
    public class AuthorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}