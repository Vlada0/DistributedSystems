using System;
using AviaSalesApi.Data.Entities;
using AviaSalesApiCopyOne.Infrastructure;
using AviaSalesApiCopyOne.Models.Tickets;
using Newtonsoft.Json;

namespace AviaSalesApiCopyOne.Data.Entities
{
    [MongoCollection("tickets")]
    public class Ticket : BaseEntity
    {
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public DateTime TakeOffDay { get; set; }
        public DateTime TakeOffDate { get; set; }
        public DateTime ArriveOn { get; set; }
        public string TransitPlaces { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
        
        public static Ticket From(TicketCreateUpdateModel src) => new Ticket
        {
            CountryFrom = src.CountryFrom,
            CityFrom = src.CityFrom,
            CountryTo = src.CountryTo,
            CityTo = src.CityTo,
            TakeOffDate = src.TakeOffDate,
            ArriveOn = src.ArriveOn,
            Company = src.Company,
            Price = src.Price,
            TransitPlaces = JsonConvert.SerializeObject(src.TransitPlaces)
        };
    }
}