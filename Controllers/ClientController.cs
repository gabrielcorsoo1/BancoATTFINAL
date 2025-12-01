using System;
using System.Linq;
using System.Threading.Tasks;
using AtlasAir.Attributes;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using AtlasAir.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtlasAir.Controllers
{
    [ClientOnly]
    public class ClientController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAirportRepository _airportRepository;

        public ClientController(
            IFlightRepository flightRepository,
            ISeatRepository seatRepository,
            IReservationRepository reservationRepository,
            IAirportRepository airportRepository)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _seatRepository = seatRepository ?? throw new ArgumentNullException(nameof(seatRepository));
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _airportRepository = airportRepository ?? throw new ArgumentNullException(nameof(airportRepository));
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.ClientFixedFooter = true;
        }


        public IActionResult Index()
        {
            return RedirectToAction(nameof(Search));
        }

        
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var flights = await _flightRepository.GetAllAsync() ?? Enumerable.Empty<Flight>();
      
            return View(flights);
        }

  
        [HttpGet]
        public async Task<IActionResult> GetFlightsByRoute(int originId, int destinationId)
        {
            var flights = Enumerable.Empty<Flight>();
            try
            {
                var method = _flightRepository.GetType().GetMethod("GetFlightsByRouteAsync");
                if (method != null)
                {
                    var task = (Task?)method.Invoke(_flightRepository, new object[] { originId, destinationId });
                    if (task != null)
                    {
                        await task.ConfigureAwait(false);
                        var resultProp = task.GetType().GetProperty("Result");
                        flights = (IEnumerable<Flight>?)resultProp?.GetValue(task) ?? Enumerable.Empty<Flight>();
                    }
                    else
                    {
                        flights = Enumerable.Empty<Flight>();
                    }
                }
                else
                {
                    flights = (await _flightRepository.GetAllAsync())?
                        .Where(f => f.OriginAirportId == originId && f.DestinationAirportId == destinationId)
                        ?? Enumerable.Empty<Flight>();
                }
            }
            catch
            {
                flights = (await _flightRepository.GetAllAsync())?
                    .Where(f => f.OriginAirportId == originId && f.DestinationAirportId == destinationId)
                    ?? Enumerable.Empty<Flight>();
            }

            var data = flights.Select(f => new
            {
                f.Id,
                Origin = f.OriginAirport?.Name,
                Destination = f.DestinationAirport?.Name,
                Departure = f.ScheduledDeparture.ToString("g"),
                Arrival = f.ScheduledArrival.ToString("g")
            });

            return Json(data);
        }

        
        [HttpGet]
        public async Task<IActionResult> Seats(int? id)
        {
            
            if (!id.HasValue || id.Value <= 0)
            {
                return RedirectToAction(nameof(Search));
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null) return NotFound();

            var seats = await _seatRepository.GetAvailableSeatsByFlightIdAsync(id.Value) ?? Enumerable.Empty<Seat>();

            ViewData["Flight"] = flight;
            return View(seats);
        }

        // Compra
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(int flightId, int seatId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Seats", "Client", new { id = flightId }) });
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

            await _reservationRepository.CreateAsync(reservation);
            TempData["SuccessMessage"] = $"Reserva criada: {reservation.ReservationCode}";

            // redireciona para a tela pré-pagamento (agora mostra "Sua reserva está quase pronta")
            return RedirectToAction(nameof(PaymentConfirmation), new { code = reservation.ReservationCode });
        }

        public IActionResult DashboardTest()
        {
            return View();
        }

      
        [HttpGet]
        public async Task<IActionResult> Payment(int flightId, int seatId)
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (flight == null || seat == null) return NotFound();

            var vm = new PaymentViewModel
            {
                FlightId = flightId,
                SeatId = seatId,
                Flight = flight,
                Seat = seat,
                Price = 199.90m
            };

            return View(vm);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentFormModel form)
        {
            if (!ModelState.IsValid)
            {
                
                var flight = await _flightRepository.GetByIdAsync(form.FlightId);
                var seat = await _seatRepository.GetByIdAsync(form.SeatId);
                var vm = new PaymentViewModel
                {
                    FlightId = form.FlightId,
                    SeatId = form.SeatId,
                    Flight = flight,
                    Seat = seat,
                    Price = 199.90m
                };
                return View(vm);
            }

            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Seats", "Client", new { id = form.FlightId }) });
            }

            var reservation = new Reservation
            {
                ReservationCode = $"R-{Guid.NewGuid().ToString().Split('-')[0].ToUpper()}",
                CustomerId = customerId.Value,
                FlightId = form.FlightId,
                SeatId = form.SeatId,
                ReservationDate = DateTime.UtcNow,
                Status = AtlasAir.Enums.ReservationStatus.Confirmed
            };

            await _reservationRepository.CreateAsync(reservation);

            // redireciona para confirmação de pagamento
            return RedirectToAction(nameof(PaymentConfirmation), new { code = reservation.ReservationCode });
        }

        [HttpGet]
        public IActionResult PaymentConfirmation(string code)
        {
            ViewBag.ReservationCode = code;
            return View();
        }

        
        [HttpGet]
        public IActionResult PaymentPage(string code)
        {
            ViewBag.ReservationCode = code;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentPage(string code, string cardHolder, string cardNumber, string expiry, string cvv)
        {
            
            return RedirectToAction(nameof(ReservationConfirmed), new { code });
        }

        [HttpGet]
        public IActionResult ReservationConfirmed(string code)
        {
            ViewBag.ReservationCode = code;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickReserve(int flightId)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "1";
            if (isAdmin)
            {
                
                return Json(new { success = false, message = "Ação não disponível para administradores." });
            }

            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
              
                return Json(new { success = false, needsLogin = true, redirect = Url.Action("Login", "Account", new { returnUrl = Url.Action("Seats", "Client", new { id = flightId }) }) });
            }

            var availableSeats = await _seatRepository.GetAvailableSeatsByFlightIdAsync(flightId) ?? Enumerable.Empty<Seat>();
            var seatsList = availableSeats.ToList();
            if (!seatsList.Any())
            {
                return Json(new { success = false, message = "Nenhum assento disponível." });
            }

            var rnd = new Random();
            var seat = seatsList[rnd.Next(seatsList.Count)];

            var reservation = new Reservation
            {
                ReservationCode = $"R-{Guid.NewGuid().ToString().Split('-')[0].ToUpper()}",
                CustomerId = customerId.Value,
                FlightId = flightId,
                SeatId = seat.Id,
                ReservationDate = DateTime.UtcNow,
                Status = AtlasAir.Enums.ReservationStatus.Confirmed
            };

            await _reservationRepository.CreateAsync(reservation);

            return Json(new
            {
                success = true,
                seatNumber = seat.SeatNumber,
                reservationCode = reservation.ReservationCode,
                reservationId = reservation.Id
            });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                return Json(new { success = false, message = "Reserva não encontrada." });

            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "1";
            if (!isAdmin && (!customerId.HasValue || customerId.Value != reservation.CustomerId))
            {
                return Json(new { success = false, message = "Não autorizado a cancelar esta reserva." });
            }

            reservation.Status = AtlasAir.Enums.ReservationStatus.Cancelled;
            reservation.CancellationDate = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(reservation);

            return Json(new { success = true });
        }

        
    }
}