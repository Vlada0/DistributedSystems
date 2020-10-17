using System;

namespace AviaSalesApi.Data.Entities
{
    public class WarrantByPassengerIdnoAndTicketId : BaseEntity
    {
        public string PassengerIdno { get; set; }
        public string PassportId { get; set; }
        public Guid TicketId { get; set; }
        public Guid? TicketBackId { get; set; }
        public bool IsPaid { get; set; }
    }
}