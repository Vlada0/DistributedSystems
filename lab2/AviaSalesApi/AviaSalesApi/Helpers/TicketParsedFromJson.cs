using System;
using AviaSalesApi.Data.Entities;
using Newtonsoft.Json;
// ReSharper disable All

namespace AviaSalesApi.Helpers
{
    public class TicketParsedFromJson
    {
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public DateTime TakeOffDay { get; set; }
        public DateTime TakeOffDate { get; set; }
        public DateTime ArriveOn { get; set; }
        public TransitLocation[] TransitPlaces { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }

        public static Ticket From(TicketParsedFromJson src) => new Ticket
        {
            CountryFrom = src.CountryFrom,
            CityFrom = src.CityFrom,
            CountryTo = src.CountryTo,
            CityTo = src.CityTo,
            TakeOffYear = src.TakeOffDay.Year,
            TakeOffMonth = src.TakeOffDay.Month,
            TakeOffDay = src.TakeOffDay.Day,
            TakeOffDate = src.TakeOffDate,
            ArriveOn = src.ArriveOn,
            Company = src.Company,
            Price = src.Price,
            TransitPlaces = JsonConvert.SerializeObject(src.TransitPlaces)
        };
    }
    
    public class TransitLocation
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Airport { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime TakeOff { get; set; }
    }
}