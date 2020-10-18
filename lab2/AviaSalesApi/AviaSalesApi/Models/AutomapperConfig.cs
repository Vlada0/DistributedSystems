using System;
using AutoMapper;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Models.Tickets;
using Newtonsoft.Json;

namespace AviaSalesApi.Models
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Ticket, TicketModel>()
                .ForMember(dest => dest.TransitPlaces,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TakeOffDay, opt => opt.MapFrom(
                    src => new DateTime(src.TakeOffYear, src.TakeOffMonth, src.TakeOffDay)));
            
            CreateMap<TicketById, TicketModel>()
                .ForMember(dest => dest.TransitPlaces,
                    opt => opt.MapFrom(
                        src => JsonConvert.DeserializeObject<TransitPlace[]>(src.TransitPlaces)))
                .ForMember(dest => dest.TakeOffDay, opt => opt.MapFrom(
                    src => new DateTime(src.TakeOffYear, src.TakeOffMonth, src.TakeOffDay)));
        }
    }
}