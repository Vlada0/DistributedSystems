using System;

namespace AviaSalesApi.Data.Entities
{
    public class WarrantByPassengerIban : BaseEntity
    {
        public string PassengerIban { get; set; }
        public string PassportId { get; set; }
        public Guid TicketId { get; set; }
        public Guid? TicketBackId { get; set; }
        public bool IsPaid { get; set; }
    }
}