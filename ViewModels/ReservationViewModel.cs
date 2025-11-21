using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AtlasAir.ViewModels
{
    public class ReservationViewModel
    {

        [Display(Name = "Aeroporto de Origem")]
        [Required(ErrorMessage = "Selecione um aeroporto de origem.")]
        public int? SelectedOriginAirportId { get; set; }

        [Display(Name = "Aeroporto de Destino")]
        [Required(ErrorMessage = "Selecione um aeroporto de destino.")]
        public int? SelectedDestinationAirportId { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Selecione um cliente.")]
        public int? SelectedCustomerId { get; set; }

        [Display(Name = "Voo Selecionado")]
        [Required(ErrorMessage = "Selecione um voo da lista.")]
        public int? SelectedFlightId { get; set; }

        [Display(Name = "Assento Selecionado")]
        [Required(ErrorMessage = "Selecione um assento.")]
        public int? SelectedSeatId { get; set; }

        [Display(Name = "Código da Reserva")]
        [Required(ErrorMessage = "O código é obrigatório.")]
        
        public string ReservationCode { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> AirportList { get; set; }
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public IEnumerable<AvailableFlight> AvailableFlights { get; set; }
        public IEnumerable<SelectListItem> SeatList { get; set; }
        public ReservationViewModel()
        {
            AirportList = [];
            CustomerList = [];
            AvailableFlights = [];
            SeatList = [];
        }

        public class AvailableFlight
        {
            public int FlightId { get; set; }
            public string FlightNumber { get; set; } = string.Empty;
            public string OriginAirportName { get; set; } = string.Empty;
            public string DestinationAirportName { get; set; } = string.Empty;
            public DateTime ScheduledDeparture { get; set; }
            public DateTime ScheduledArrival { get; set; }
        }
    }
}


