using AtlasAir.Enums;
using AtlasAir.Models;
using AtlasAir.Services;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AtlasAirDbContext context)
        {
            // Aplica migrations pendentes ao iniciar a aplicação.
            context.Database.Migrate();

            // Garantir que exista um administrador — útil em DB já existente
            if (!context.Customers.Any(c => c.IsAdmin))
            {
                var adminEmail = "admin@atlasair.local";
                // não criar duplicado se já existir com esse email
                if (!context.Customers.Any(c => c.Email == adminEmail))
                {
                    var admin = new Customer
                    {
                        Name = "Administrador",
                        Email = adminEmail,
                        Phone = "",
                        PasswordHash = PasswordHasher.Hash("Admin@123"), // senha inicial — altere depois
                        IsAdmin = true,
                        IsSpecialCustomer = false
                    };
                    context.Customers.Add(admin);
                    context.SaveChanges();
                }
            }

            if (context.Airports.Any())
            {
                return; // já semeado
            }

            var gru = new Airport
            {
                Name = "São Paulo/Guarulhos - GRU",
                Street = "Rod. Hélio Smidt",
                Neighborhood = "Cumbica",
                City = "Guarulhos",
                State = "SP",
                Country = "Brazil",
                ZipCode = "07190-100"
            };

            var gig = new Airport
            {
                Name = "Rio de Janeiro/Galeão - GIG",
                Street = "Av. Vinte de Janeiro",
                Neighborhood = "Ilha do Governador",
                City = "Rio de Janeiro",
                State = "RJ",
                Country = "Brazil",
                ZipCode = "21941-590"
            };

            var jfk = new Airport
            {
                Name = "John F. Kennedy International - JFK",
                Street = "JFK Expy",
                Neighborhood = "Queens",
                City = "New York",
                State = "NY",
                Country = "USA",
                ZipCode = "11430"
            };

            context.Airports.AddRange(gru, gig, jfk);
            context.SaveChanges();

            var boeing737 = new Aircraft { Model = "Boeing 737-800", SeatCount = 66 };
            var a320 = new Aircraft { Model = "Airbus A320", SeatCount = 62 };

            context.Aircrafts.AddRange(boeing737, a320);
            context.SaveChanges();

            var seats = new List<Seat>();
            foreach (var ac in new[] { boeing737, a320 })
            {
                for (int i = 1; i <= ac.SeatCount; i++)
                {
                    var row = ((i - 1) / 3) + 1;
                    var col = (char)('A' + ((i - 1) % 3));
                    var seatNumber = $"{row}{col}";

                    seats.Add(new Seat
                    {
                        AircraftId = ac.Id,
                        SeatNumber = seatNumber,
                        Class = i <= 2 ? SeatClass.Business : SeatClass.Economy
                    });
                }
            }

            context.Seats.AddRange(seats);
            context.SaveChanges();

            var customer1 = new Customer { Name = "Carlos Silva", Phone = "+55 11 91234-5678", IsSpecialCustomer = false };
            var customer2 = new Customer { Name = "Ana Pereira", Phone = "+55 21 99876-5432", IsSpecialCustomer = true };

            context.Customers.AddRange(customer1, customer2);
            context.SaveChanges();

            var flight1 = new Flight
            {
                OriginAirportId = gru.Id,
                DestinationAirportId = jfk.Id,
                ScheduledDeparture = DateTime.UtcNow.AddDays(2).AddHours(6),
                ScheduledArrival = DateTime.UtcNow.AddDays(2).AddHours(16),
                Status = FlightStatus.Scheduled
            };

            var flight2 = new Flight
            {
                OriginAirportId = gig.Id,
                DestinationAirportId = gru.Id,
                ScheduledDeparture = DateTime.UtcNow.AddDays(1).AddHours(9),
                ScheduledArrival = DateTime.UtcNow.AddDays(1).AddHours(11),
                Status = FlightStatus.Scheduled
            };

            context.Flights.AddRange(flight1, flight2);
            context.SaveChanges();

            var segment1 = new FlightSegment
            {
                FlightId = flight1.Id,
                AircraftId = boeing737.Id,
                SegmentOrder = 1,
                OriginAirportId = gru.Id,
                DestinationAirportId = jfk.Id,
                DepartureTime = flight1.ScheduledDeparture,
                ArrivalTime = flight1.ScheduledArrival
            };

            var segment2 = new FlightSegment
            {
                FlightId = flight2.Id,
                AircraftId = a320.Id,
                SegmentOrder = 1,
                OriginAirportId = gig.Id,
                DestinationAirportId = gru.Id,
                DepartureTime = flight2.ScheduledDeparture,
                ArrivalTime = flight2.ScheduledArrival
            };

            context.FlightSegments.AddRange(segment1, segment2);
            context.SaveChanges();

            var reservation1 = new Reservation
            {
                ReservationCode = $"R-{Guid.NewGuid().ToString().Split('-')[0]}",
                CustomerId = customer1.Id,
                SeatId = seats.First(s => s.AircraftId == boeing737.Id).Id,
                FlightId = flight1.Id,
                Status = ReservationStatus.Confirmed,
                ReservationDate = DateTime.UtcNow
            };

            var reservation2 = new Reservation
            {
                ReservationCode = $"R-{Guid.NewGuid().ToString().Split('-')[0]}",
                CustomerId = customer2.Id,
                SeatId = seats.First(s => s.AircraftId == a320.Id).Id,
                FlightId = flight2.Id,
                Status = ReservationStatus.Confirmed,
                ReservationDate = DateTime.UtcNow
            };

            context.Reservations.AddRange(reservation1, reservation2);
            context.SaveChanges();
        }
    }
}
