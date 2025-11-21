using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtlasAir.Models
{
    public class FlightSegment
    {
        [Key]
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int AircraftId { get; set; }
        public byte SegmentOrder { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }
        [ForeignKey("AircraftId")]
        public Aircraft? Aircraft { get; set; }
        [ForeignKey("OriginAirportId")]
        public Airport? OriginAirport { get; set; }
        [ForeignKey("DestinationAirportId")]
        public Airport? DestinationAirport { get; set; }
    }
}
