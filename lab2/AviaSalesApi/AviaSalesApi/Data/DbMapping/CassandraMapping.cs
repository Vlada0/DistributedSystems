using AviaSalesApi.Data.Entities;
using Cassandra.Mapping;

namespace AviaSalesApi.Data.DbMapping
{
    public class CassandraMapping : Mappings
    {
        public CassandraMapping()
        {
            For<Ticket>()
                .TableName("ticketbyplacefromplacetotakeoffarrivedate")
                .Column(t => t.CountryFrom, c => c.WithName("country_from"))
                .Column(t => t.CityFrom, c => c.WithName("city_from"))
                .Column(t => t.CountryTo, c => c.WithName("country_to"))
                .Column(t => t.CityTo, c => c.WithName("city_to"))
                .Column(t => t.TakeOffDate, c => c.WithName("takeoff_date"))
                .Column(t => t.ArriveOn, c => c.WithName("arrive_on"))
                .Column(t => t.TransitPlaces, c => c.WithName("transit_places"));
            
            For<TicketById>()
                .TableName("ticket_by_id")
                .Column(t => t.CountryFrom, c => c.WithName("country_from"))
                .Column(t => t.CityFrom, c => c.WithName("city_from"))
                .Column(t => t.CountryTo, c => c.WithName("country_to"))
                .Column(t => t.CityTo, c => c.WithName("city_to"))
                .Column(t => t.TakeOffDate, c => c.WithName("takeoff_date"))
                .Column(t => t.ArriveOn, c => c.WithName("arrive_on"))
                .Column(t => t.TransitPlaces, c => c.WithName("transit_places"));

            For<WarrantByPassengerIdno>()
                .TableName("warrants_by_passenger_idno")
                .PartitionKey(w => w.PassengerIdno)
                .Column(w => w.PassengerIdno, c => c.WithName("passenger_idno"))
                .Column(w => w.PassportId, c => c.WithName("passenger_passport_id"))
                .Column(w => w.TicketId, c => c.WithName("ticket_id"))
                .Column(w => w.TicketBackId, c => c.WithName("ticket_back_id"))
                .Column(w => w.IsPaid, c => c.WithName("is_paid"));
            
            For<WarrantByPassengerIdnoAndTicketId>()
                .TableName("warrant_by_passenger_idno_and_ticket_id")
                .Column(w => w.PassengerIdno, c => c.WithName("passenger_idno"))
                .Column(w => w.PassportId, c => c.WithName("passenger_passport_id"))
                .Column(w => w.TicketId, c => c.WithName("ticket_id"))
                .Column(w => w.TicketBackId, c => c.WithName("ticket_back_id"))
                .Column(w => w.IsPaid, c => c.WithName("is_paid"));
        }
    }
}