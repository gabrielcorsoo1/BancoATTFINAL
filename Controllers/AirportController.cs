using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Controllers
{
    public class AirportController : Controller
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;

        public AirportController(
            IAirportRepository airportRepository,
            IFlightRepository flightRepository,
            IFlightSegmentRepository flightSegmentRepository)
        {
            _airportRepository = airportRepository;
            _flightRepository = flightRepository;
            _flightSegmentRepository = flightSegmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _airportRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();
            return View(airport);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Airport airport)
        {
            if (ModelState.IsValid)
            {
                await _airportRepository.CreateAsync(airport);
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();
            return View(airport);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Airport airport)
        {
            if (id != airport.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _airportRepository.UpdateAsync(airport);
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null) return NotFound();
            return View(airport);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flights = await _flightRepository.GetAllAsync() ?? Enumerable.Empty<Flight>();
            var hasFlight = flights.Any(f => f.OriginAirportId == id || f.DestinationAirportId == id);

            var segments = await _flightSegmentRepository.GetAllAsync() ?? Enumerable.Empty<FlightSegment>();
            var hasSegment = segments.Any(s => s.OriginAirportId == id || s.DestinationAirportId == id);

            if (hasFlight || hasSegment)
            {
                TempData["ErrorMessage"] = "Não foi possível excluir: existem voos ou conexões relacionados a este aeroporto.";
                return RedirectToAction(nameof(Index));
            }

            var airport = await _airportRepository.GetByIdAsync(id);
            if (airport == null) return NotFound();

            await _airportRepository.DeleteAsync(airport);
            return RedirectToAction(nameof(Index));
        }
    }
}
