using AutoMapper;
using AviaSalesApiCopyTwo.Data.Entities;
using AviaSalesApiCopyTwo.Models.Warrants;

namespace AviaSalesApiCopyTwo.Models
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