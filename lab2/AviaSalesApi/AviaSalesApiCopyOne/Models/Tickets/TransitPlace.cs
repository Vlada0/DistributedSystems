using System;

namespace AviaSalesApiCopyOne.Models.Tickets
{
    [Serializable]
    public class TransitPlace
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Airport { get; set; }
        public DateTime ArriveDate { get; set; }
        public DateTime TakeOff { get; set; }
    }
}