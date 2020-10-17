using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Data.Entities;

namespace AviaSalesApi.Helpers
{
    public interface IJsonFileProcessor
    {
        Task<IEnumerable<Ticket>> ProcessFileAsync(string filePath);
    }
}