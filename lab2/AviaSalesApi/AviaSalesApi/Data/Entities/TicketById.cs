using System;
using System.Diagnostics.CodeAnalysis;
using AviaSalesApi.Models.Tickets;
using Newtonsoft.Json;

namespace AviaSalesApi.Data.Entities
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TicketById : BaseEntity
    {
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public int TakeOffYear { get; set; }
        public int TakeOffMonth { get; set; }
        public int TakeOffDay { get; set; }
        public DateTime TakeOffDate { get; set; }
        public DateTime ArriveOn { get; set; }
        public string TransitPlaces { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }

        public static TicketById From(Ticket src) => new TicketById
        {
            Id = src.Id,
            CountryFrom = src.CountryFrom,
            CityFrom = src.CityFrom,
            CountryTo = src.CountryTo,
            CityTo = src.CityTo,
            TakeOffYear = src.TakeOffYear,
            TakeOffMonth = src.TakeOffMonth,
            TakeOffDay = src.TakeOffDay,
            TakeOffDate = src.TakeOffDate,
            ArriveOn = src.ArriveOn,
            TransitPlaces = src.TransitPlaces,
            Company = src.Company,
            Price = src.Price
        };
        
        public static TicketById From(TicketCreateUpdateModel src) => new TicketById
        {
            CountryFrom = src.CountryFrom,
            CityFrom = src.CityFrom,
            CountryTo = src.CountryTo,
            CityTo = src.CityTo,
            TakeOffDay = src.TakeOffDay.Day,
            TakeOffYear = src.TakeOffDay.Year,
            TakeOffMonth = src.TakeOffDay.Month,
            TakeOffDate = src.TakeOffDate,
            ArriveOn = src.ArriveOn,
            Company = src.Company,
            Price = src.Price,
            TransitPlaces = JsonConvert.SerializeObject(src.TransitPlaces)
        };
    }
}