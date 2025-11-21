using AtlasAir.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtlasAir.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public DateTime ScheduledDeparture { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime ScheduledArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public FlightStatus Status { get; set; }

        [ForeignKey("OriginAirportId")]
        public Airport? OriginAirport { get; set; }
        [ForeignKey("DestinationAirportId")]
        public Airport? DestinationAirport { get; set; }

        public ICollection<FlightSegment> FlightSegments { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
