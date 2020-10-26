using System;
using AviaSalesApiCopyTwo.Infrastructure;

namespace AviaSalesApiCopyTwo.Data.Entities
{
    [MongoCollection("warrants")]
    public class Warrant : BaseEntity
    {
        public string PassengerIban { get; set; }
        public string PassportId { get; set; }
        public Guid TicketId { get; set; }
        public Guid? TicketBackId { get; set; }
        public bool IsPaid { get; set; }
    }
}