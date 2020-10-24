using System;
using System.Collections.Generic;
using AutoMapper;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Models.Tickets;
using AviaSalesApi.Models.Warrants;
using Newtonsoft.Json;

namespace AviaSalesApi.Models
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Warrant, WarrantModel>();
            CreateMap<WarrantCreateUpdateModel, Warrant>();
        }
    }
}