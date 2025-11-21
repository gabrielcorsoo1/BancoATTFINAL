using AtlasAir.Enums;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using AtlasAir.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAirportRepository _airportRepository;

        public ReservationController(
            IReservationRepository reservationRepository,
            ICustomerRepository customerRepository,
            IFlightRepository flightRepository,
            ISeatRepository seatRepository,
            IAirportRepository airportRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _flightRepository = flightRepository;
            _seatRepository = seatRepository;
            _airportRepository = airportRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _reservationRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ReservationViewModel
            {
                AirportList = new SelectList(await _airportRepository.GetAllAsync(), "Id", "Name"),
                CustomerList = new SelectList(await _customerRepository.GetAllAsync(), "Id", "Name"),
            };

            return View(viewModel);
        }

        /// <summary>
        /// Ação chamada pelo JavaScript para buscar voos com base na origem e destino.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAvailableFlights(int originId, int destinationId)
        {
            var flights = await _flightRepository.GetFlightsByRouteAsync(originId, destinationId);

            var availableFlights = flights.Select(f => new ReservationViewModel.AvailableFlight
            {
                FlightId = f.Id,
                OriginAirportName = f.OriginAirport?.Name,
                DestinationAirportName = f.DestinationAirport?.Name,
                ScheduledDeparture = f.ScheduledDeparture,
                ScheduledArrival = f.ScheduledArrival
            });

            return Json(availableFlights);
        }

        /// <summary>
        /// Ação chamada pelo JavaScript para buscar assentos de um voo específico.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAvailableSeats(int flightId)
        {
            var seats = await _seatRepository.GetAvailableSeatsByFlightIdAsync(flightId);
            var seatList = seats.Select(s => new { s.Id, s.SeatNumber });
            return Json(seatList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel viewModel)
        {
            if (viewModel.SelectedCustomerId.HasValue &&
                viewModel.SelectedFlightId.HasValue &&
                viewModel.SelectedSeatId.HasValue &&
                !string.IsNullOrEmpty(viewModel.ReservationCode))
            {
                var reservation = new Reservation
                {
                    ReservationCode = viewModel.ReservationCode,
                    CustomerId = viewModel.SelectedCustomerId.Value,
                    SeatId = viewModel.SelectedSeatId.Value,
                    FlightId = viewModel.SelectedFlightId.Value,
                    ReservationDate = DateTime.Now,
                    Status = ReservationStatus.Confirmed
                };

                await _reservationRepository.CreateAsync(reservation);
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Erro ao criar reserva. Verifique todos os campos.";
            viewModel.AirportList = new SelectList(await _airportRepository.GetAllAsync(), "Id", "Name", viewModel.SelectedOriginAirportId);
            viewModel.CustomerList = new SelectList(await _customerRepository.GetAllAsync(), "Id", "Name", viewModel.SelectedCustomerId);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllAsync(), "Id", "Name", reservation.CustomerId);
            ViewData["FlightId"] = new SelectList(await _flightRepository.GetAllAsync(), "Id", "FlightNumber", reservation.FlightId);
            ViewData["SeatId"] = new SelectList(await _seatRepository.GetAllAsync(), "Id", "SeatNumber", reservation.SeatId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _reservationRepository.UpdateAsync(reservation);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllAsync(), "Id", "Name", reservation.CustomerId);
            ViewData["FlightId"] = new SelectList(await _flightRepository.GetAllAsync(), "Id", "FlightNumber", reservation.FlightId);
            ViewData["SeatId"] = new SelectList(await _seatRepository.GetAllAsync(), "Id", "SeatNumber", reservation.SeatId);
            return View(reservation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _reservationRepository.GetByIdAsync(id.Value);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return NotFound();

            try
            {
                await _reservationRepository.DeleteAsync(reservation);
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Não foi possível excluir, pois existem dados relacionados.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}