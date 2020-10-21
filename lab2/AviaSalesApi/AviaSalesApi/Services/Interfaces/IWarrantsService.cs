using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Models.Warrants;

namespace AviaSalesApi.Services.Interfaces
{
    public interface IWarrantsService
    {
        Task<IEnumerable<WarrantModel>> GetWarrantsByIban(string iban);
        Task<WarrantModel> GetWarrantByIbanAndId(string iban, Guid id);

        Task<WarrantModel> CreateWarrant(WarrantCreateUpdateModel model);
    }
}