using System;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Helpers;

namespace AviaSalesApi.Data
{
    public static class Seed
    {
        public static async Task SeedToMongoAsync(IMongoRepository<Ticket> repository, IJsonFileProcessor processor)
        {
            var tickets = await processor.ProcessFileAsync(
                @"D:\Универ\4 курс\1 семестр\PAD\LABS\DistributedSystems\lab2\AviaSalesApi\AviaSalesApi\seed.json");
            
            foreach (var ticket in tickets)
            {
                ticket.Id = Guid.NewGuid();
                await repository.InsertOneAsync(ticket);
            }
        }
    }
}