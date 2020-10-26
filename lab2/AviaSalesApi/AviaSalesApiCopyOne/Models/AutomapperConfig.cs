using AutoMapper;
using AviaSalesApi.Data.Entities;
using AviaSalesApiCopyOne.Models.Warrants;

namespace AviaSalesApiCopyOne.Models
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