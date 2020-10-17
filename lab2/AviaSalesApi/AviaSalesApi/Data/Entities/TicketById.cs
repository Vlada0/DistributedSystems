using System;

namespace AviaSalesApi.Data.Entities
{
    public class TicketById : BaseEntity
    {
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public DateTime TakeOffDate { get; set; }
        public DateTime ArriveOn { get; set; }
        public string TransitPlaces { get; set; }
        public int Price { get; set; }
    }
}