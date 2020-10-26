using System;
using System.Collections.Generic;
using AviaSalesApiCopyOne.Data.Entities;
using Newtonsoft.Json;

namespace AviaSalesApiCopyOne.Models.Tickets
{
    public class TicketModel
    {
        public Guid Id { get; set; }
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public DateTime TakeOffDay { get; set; }
        public DateTime TakeOffDate { get; set; }
        public DateTime ArriveOn { get; set; }
        public IEnumerable<TransitPlace> TransitPlaces { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
        
        public static TicketModel From(Ticket src) => new TicketModel
        {
            Id = src.Id,
            CountryFrom = src.CountryFrom,
            CityFrom = src.CityFrom,
            CountryTo = src.CountryTo,
            CityTo = src.CityTo,
            TakeOffDay = src.TakeOffDay,
            TakeOffDate = src.TakeOffDate,
            ArriveOn = src.ArriveOn,
            TransitPlaces = JsonConvert.DeserializeObject<IEnumerable<TransitPlace>>(src.TransitPlaces),
            Company = src.Company,
            Price = src.Price
        };
    }
}