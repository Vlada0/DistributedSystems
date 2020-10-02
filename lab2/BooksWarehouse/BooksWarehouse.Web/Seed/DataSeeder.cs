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
        public static async Task SeedCountries(IMongoDbRepository<Country> repository)
        {
            var countries = new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Russia",
                    Code = "ru"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "France",
                    Code = "fr"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Germany",
                    Code = "de"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Great Britain",
                    Code = "gb"
                },
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Spain",
                    Code = "esp"
                }
            };

            await repository.InsertManyAsync(countries);
        }

        public static async Task SeedAuthors(IMongoDbRepository<Author> authorsRepository, 
            IMongoDbRepository<Country> countriesRepository)
        {
            var countries = await countriesRepository.GetListAsync();

            var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alexandr",
                    LastName = "Pushkin",
                    BirthDate = new DateTime(1799, 5, 26),
                    DateOfDeath = new DateTime(1837, 1, 29),
                    CountryId = countries.FirstOrDefault(c => c.Code == "ru").Id
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Николай",
                    LastName = "Гоголь",
                    BirthDate = new DateTime(1809, 4, 1),
                    DateOfDeath = new DateTime(1852, 3, 4),
                    CountryId = countries.FirstOrDefault(c => c.Code == "ru").Id
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Антон",
                    LastName = "Чехов",
                    BirthDate = new DateTime(1860, 1, 29),
                    DateOfDeath = new DateTime(1904, 7, 15),
                    CountryId = countries.FirstOrDefault(c => c.Code == "ru").Id
                }
            };

            await authorsRepository.InsertManyAsync(authors);
        }
    }
}