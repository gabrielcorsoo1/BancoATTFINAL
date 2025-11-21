using AtlasAir.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AtlasAir.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public SeatClass Class { get; set; }

        [ForeignKey("AircraftId")]
        public Aircraft Aircraft { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Não mapeado para o banco para evitar erro 'Invalid column name RowVersion'
        [NotMapped]
        public byte[]? RowVersion { get; set; }
    }
}
