using AtlasAir.Enums;
using AtlasAir.Helpers;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Controllers
{
    public class FlightSegmentController(IFlightSegmentRepository flightSegmentRepository, IAircraftRepository aircraftRepository, IAirportRepository airportRepository, IFlightRepository flightRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await flightSegmentRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightSegment = await flightSegmentRepository.GetByIdAsync(id.Value);
            if (flightSegment == null)
            {
                return NotFound();
            }

            return View(flightSegment);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["AircraftId"] = new SelectList(await aircraftRepository.GetAllAsync(), "Id", "Id");
            ViewData["DestinationAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id");
            ViewData["FlightId"] = new SelectList(await flightRepository.GetAllAsync(), "Id", "Id");
            ViewData["OriginAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlightSegment flightSegment)
        {
            if (ModelState.IsValid)
            {
                await flightSegmentRepository.CreateAsync(flightSegment);
                return RedirectToAction(nameof(Index));
            }

            ViewData["AircraftId"] = new SelectList(await aircraftRepository.GetAllAsync(), "Id", "Id", flightSegment.AircraftId);
            ViewData["DestinationAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.DestinationAirportId);
            ViewData["FlightId"] = new SelectList(await flightRepository.GetAllAsync(), "Id", "Id", flightSegment.FlightId);
            ViewData["OriginAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.OriginAirportId);
            return View(flightSegment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightSegment = await flightSegmentRepository.GetByIdAsync(id.Value);
            if (flightSegment == null)
            {
                return NotFound();
            }

            ViewData["AircraftId"] = new SelectList(await aircraftRepository.GetAllAsync(), "Id", "Id", flightSegment.AircraftId);
            ViewData["DestinationAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.DestinationAirportId);
            ViewData["FlightId"] = new SelectList(await flightRepository.GetAllAsync(), "Id", "Id", flightSegment.FlightId);
            ViewData["OriginAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.OriginAirportId);
            return View(flightSegment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FlightSegment flightSegment)
        {
            if (id != flightSegment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await flightSegmentRepository.UpdateAsync(flightSegment);
                return RedirectToAction(nameof(Index));
            }

            ViewData["AircraftId"] = new SelectList(await aircraftRepository.GetAllAsync(), "Id", "Id", flightSegment.AircraftId);
            ViewData["DestinationAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.DestinationAirportId);
            ViewData["FlightId"] = new SelectList(await flightRepository.GetAllAsync(), "Id", "Id", flightSegment.FlightId);
            ViewData["OriginAirportId"] = new SelectList(await airportRepository.GetAllAsync(), "Id", "Id", flightSegment.OriginAirportId);

            var statusValues = Enum.GetValues(typeof(FlightStatus)).Cast<FlightStatus>();
            var statusOptions = statusValues.Select(s => new SelectListItem
            {
                Text = s.GetDisplayName(),
                Value = s.ToString()
            });

            ViewData["StatusOptions"] = new SelectList(statusOptions, "Value", "Text");

            return View(flightSegment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightSegment = await flightSegmentRepository.GetByIdAsync(id.Value);
            if (flightSegment == null)
            {
                return NotFound();
            }

            return View(flightSegment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flightSegment = await flightSegmentRepository.GetByIdAsync(id);
            if (flightSegment == null)
            {
                return NotFound(); ;
            }

            try
            {
                await flightSegmentRepository.DeleteAsync(flightSegment);
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Não foi possível excluir, pois existem dados relacionados.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
