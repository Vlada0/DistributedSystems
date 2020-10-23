using AviaSalesApi.Data.Entities;
using Cassandra.Mapping;

namespace AviaSalesApi.Data.DbMapping
{
    public class CassandraMapping : Mappings
    {
        public CassandraMapping()
        {
            For<Ticket1>()
                .TableName("ticket_by_place_from_place_to_takeoff_day")
                .Column(t => t.CountryFrom, c => c.WithName("country_from"))
                .Column(t => t.CityFrom, c => c.WithName("city_from"))
                .Column(t => t.CountryTo, c => c.WithName("country_to"))
                .Column(t => t.CityTo, c => c.WithName("city_to"))
                .Column(t => t.TakeOffYear, c => c.WithName("takeoff_year"))
                .Column(t => t.TakeOffMonth, c => c.WithName("takeoff_month"))
                .Column(t => t.TakeOffDay, c => c.WithName("takeoff_day"))
                .Column(t => t.TakeOffDate, c => c.WithName("takeoff_date"))
                .Column(t => t.ArriveOn, c => c.WithName("arrive_date"))
                .Column(t => t.TransitPlaces, c => c.WithName("transit_places"));
            
            For<TicketById>()
                .TableName("ticket_by_id")
                .Column(t => t.CountryFrom, c => c.WithName("country_from"))
                .Column(t => t.CityFrom, c => c.WithName("city_from"))
                .Column(t => t.CountryTo, c => c.WithName("country_to"))
                .Column(t => t.CityTo, c => c.WithName("city_to"))
                .Column(t => t.TakeOffYear, c => c.WithName("takeoff_year"))
                .Column(t => t.TakeOffMonth, c => c.WithName("takeoff_month"))
                .Column(t => t.TakeOffDay, c => c.WithName("takeoff_day"))
                .Column(t => t.TakeOffDate, c => c.WithName("takeoff_date"))
                .Column(t => t.ArriveOn, c => c.WithName("arrive_date"))
                .Column(t => t.TransitPlaces, c => c.WithName("transit_places"));

            For<WarrantByPassengerIban>()
                .TableName("warrants_by_passenger_iban")
                .PartitionKey(w => w.PassengerIban)
                .Column(w => w.PassengerIban, c => c.WithName("passenger_iban"))
                .Column(w => w.PassportId, c => c.WithName("passenger_passport_id"))
                .Column(w => w.TicketId, c => c.WithName("ticket_id"))
                .Column(w => w.TicketBackId, c => c.WithName("ticket_back_id"))
                .Column(w => w.IsPaid, c => c.WithName("is_paid"));
            
            For<WarrantByPassengerIbanAndTicketId>()
                .TableName("warrant_by_passenger_iban_and_ticket_id")
                .Column(w => w.PassengerIban, c => c.WithName("passenger_iban"))
                .Column(w => w.PassportId, c => c.WithName("passenger_passport_id"))
                .Column(w => w.TicketId, c => c.WithName("ticket_id"))
                .Column(w => w.TicketBackId, c => c.WithName("ticket_back_id"))
                .Column(w => w.IsPaid, c => c.WithName("is_paid"));
        }
    }
}