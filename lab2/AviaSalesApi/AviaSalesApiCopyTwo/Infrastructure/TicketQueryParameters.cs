using System;

namespace AviaSalesApiCopyTwo.Infrastructure
{
    public class TicketQueryParameters
    {
        public string CountryFrom { get; set; }
        public string CityFrom { get; set; }
        public string CountryTo { get; set; }
        public string CityTo { get; set; }
        public DateTime? TakeOffDay { get; set; }
    }
}