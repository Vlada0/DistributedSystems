using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;
using Newtonsoft.Json;

namespace AviaSalesApi.Helpers
{
    public class JsonFileProcessor : IJsonFileProcessor
    {
        public async Task<IEnumerable<Ticket>> ProcessFileAsync(string filePath)
        {
            var tickets = new List<Ticket>();
            var serializer = new JsonSerializer();
            await using var fs = File.Open(filePath, FileMode.Open);
            using var sr = new StreamReader(fs);
            using JsonReader jReader = new JsonTextReader(sr);
            while (await jReader.ReadAsync())
            {
                if (jReader.TokenType == JsonToken.StartObject)
                {
                    var ticket = serializer.Deserialize<TicketParsedFromJson>(jReader);
                    var parsedTicked = TicketParsedFromJson.From(ticket);
                                
                    tickets.Add(parsedTicked);
                }
            }

            return tickets;
        }
    }
}