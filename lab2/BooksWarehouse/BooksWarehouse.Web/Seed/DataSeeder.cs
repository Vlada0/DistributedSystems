using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksWarehouse.Domain.Entities;
using BooksWarehouse.Infrastructure.Data;

namespace BooksWarehouse.Web.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAuthors(IMongoDbRepository<Author> authorsRepository)
        {
            var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alexandr",
                    LastName = "Pushkin",
                    BirthDate = new DateTime(1799, 5, 26),
                    DateOfDeath = new DateTime(1837, 1, 29),
                    Country = "Russia"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Николай",
                    LastName = "Гоголь",
                    BirthDate = new DateTime(1809, 4, 1),
                    DateOfDeath = new DateTime(1852, 3, 4),
                    Country = "Russia"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Антон",
                    LastName = "Чехов",
                    BirthDate = new DateTime(1860, 1, 29),
                    DateOfDeath = new DateTime(1904, 7, 15),
                    Country = "Russia"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Tolkien",
                    BirthDate = new DateTime(1892, 1, 3),
                    DateOfDeath = new DateTime(1973, 9, 2),
                    Country = "Russia"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Victor",
                    LastName = "Hugo",
                    BirthDate = new DateTime(1802, 2, 26),
                    DateOfDeath = new DateTime(1885, 5, 22),
                    Country = "Russia"
                }
            };

            await authorsRepository.InsertManyAsync(authors);
        }
    }
}