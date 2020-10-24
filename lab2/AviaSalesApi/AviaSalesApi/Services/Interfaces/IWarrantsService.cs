﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Models.Warrants;

namespace AviaSalesApi.Services.Interfaces
{
    public interface IWarrantsService
    {
        Task<IEnumerable<WarrantModel>> GetWarrantsByIbanAsync(string iban);
        Task<WarrantModel> GetWarrantByIdAsync(Guid id);

        Task<WarrantModel> CreateWarrantAsync(WarrantCreateUpdateModel model);
        Task UpdateWarrantAsync(Guid warrantId, WarrantCreateUpdateModel model);
        Task DeleteWarrantAsync(Guid warrantId);
    }
}