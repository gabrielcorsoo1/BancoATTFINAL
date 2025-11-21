using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.AspNetCore.Mvc;

namespace AtlasAir.Controllers
{
    public class ClientController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IReservationRepository _reservationRepository;

        public ClientController(IFlightRepository flightRepository, ISeatRepository seatRepository, IReservationRepository reservationRepository)
        {
            _flightRepository = flightRepository;
            _seat_repository = seatRepository;
            _reservation_repository = reservationRepository;
        }

        // Lista voos disponíveis
        public async Task<IActionResult> Index()
        {
            var flights = await _flightRepository.GetAllAsync();
            return View(flights);
        }

        // Detalhes do voo + assentos disponíveis
        public async Task<IActionResult> Details(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null) return NotFound();

            var seats = await _seat_repository.GetAvailableSeatsByFlightIdAsync(id);
            ViewData["AvailableSeats"] = seats;
            return View(flight);
        }

        // Compra (posta uma reserva)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int flightId, int seatId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Client", new { id = flightId }) });
            }

            var reservation = new Reservation
            {
                ReservationCode = $"R-{Guid.NewGuid().ToString().Split('-')[0].ToUpper()}",
                CustomerId = customerId.Value,
                FlightId = flightId,
                SeatId = seatId,
                ReservationDate = DateTime.UtcNow,
                Status = AtlasAir.Enums.ReservationStatus.Confirmed
            };

            await _reservation_repository.CreateAsync(reservation);
            TempData["SuccessMessage"] = $"Reserva criada: {reservation.ReservationCode}";
            return RedirectToAction("Index");
        }

        // Página simples de teste para cliente (nova)
        public IActionResult DashboardTest()
        {
            // Apenas uma página simples para verificar redirecionamento de cliente
            return View();
        }
    }
}