using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtlasAir.Models
{
    public class Aircraft
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int SeatCount { get; set; }

        // inicialização clássica, mais clara para apresentação
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
    }
}
