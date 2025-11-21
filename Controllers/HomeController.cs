using System.Threading.Tasks;
using AtlasAir.Interfaces;
using AtlasAir.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AtlasAir.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;
        private readonly IReservationRepository _reservationRepository;

        public HomeController(
            IAircraftRepository aircraftRepository,
            IAirportRepository airportRepository,
            ICustomerRepository customerRepository,
            IFlightRepository flightRepository,
            IFlightSegmentRepository flightSegmentRepository,
            IReservationRepository reservationRepository)
        {
            _aircraftRepository = aircraftRepository;
            _airportRepository = airportRepository;
            _customerRepository = customerRepository;
            _flightRepository = flightRepository;
            _flightSegmentRepository = flightSegmentRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var aircrafts = await _aircraftRepository.GetAllAsync();
            var airports = await _airportRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            var flights = await _flightRepository.GetAllAsync();
            var segments = await _flightSegmentRepository.GetAllAsync();
            var reservations = await _reservationRepository.GetAllAsync();

            var model = new HomeIndexViewModel
            {
                AircraftCount = aircrafts?.Count ?? 0,
                AirportCount = airports?.Count ?? 0,
                CustomerCount = customers?.Count ?? 0,
                FlightCount = flights?.Count ?? 0,
                FlightSegmentCount = segments?.Count ?? 0,
                ReservationCount = reservations?.Count ?? 0
            };

            return View(model);
        }
    }
}
