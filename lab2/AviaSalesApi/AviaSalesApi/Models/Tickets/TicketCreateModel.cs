using System;
using System.Collections.Generic;

namespace AviaSalesApi.Models.Tickets
{
    public class TicketCreateModel
    {
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
    }
}