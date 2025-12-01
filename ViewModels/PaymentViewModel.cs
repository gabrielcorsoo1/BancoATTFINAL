using AtlasAir.Models;

namespace AtlasAir.ViewModels
{
    public class PaymentViewModel
    {
        public int FlightId { get; set; }
        public int SeatId { get; set; }
        public Flight? Flight { get; set; }
        public Seat? Seat { get; set; }
        public decimal Price { get; set; } = 199.90m; 
    }

    public class PaymentFormModel
    {
        public int FlightId { get; set; }
        public int SeatId { get; set; }
        public string CardHolder { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiration { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}