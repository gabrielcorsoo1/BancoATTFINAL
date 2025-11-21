using AtlasAir.Enums;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Data
{
    public class AtlasAirDbContext : DbContext
    {
        public AtlasAirDbContext(DbContextOptions<AtlasAirDbContext> options) : base(options) { }

        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aircraft>().ToTable("Aircraft");
            
            modelBuilder.Entity<Airport>().ToTable("Airport");

            modelBuilder.Entity<Customer>().ToTable("Customer");
            
            modelBuilder.Entity<Flight>().ToTable("Flight");

            modelBuilder.Entity<Flight>()
                .Property(f => f.Status)
                .HasConversion<string>()
                .HasDefaultValue(FlightStatus.Scheduled);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginAirport)
                .WithMany()
                .HasForeignKey(b => b.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationAirport)
                .WithMany()
                .HasForeignKey(b => b.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightSegment>().ToTable("FlightSegment");
            
            modelBuilder.Entity<FlightSegment>()
                .HasOne(i => i.Flight)
                .WithMany()
                .HasForeignKey(i => i.FlightId);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(i => i.Aircraft)
                .WithMany()
                .HasForeignKey(i => i.AircraftId);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(i => i.OriginAirport)
                .WithMany()
                .HasForeignKey(i => i.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(i => i.DestinationAirport)
                .WithMany()
                .HasForeignKey(i => i.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().ToTable("Reservation");

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>()
                .HasDefaultValue(ReservationStatus.Pending);

            modelBuilder.Entity<Reservation>()
                .HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Reservation>()
                .HasOne(b => b.Seat)
                .WithMany()
                .HasForeignKey(b => b.SeatId);

            modelBuilder.Entity<Reservation>()
                .HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightId);

            modelBuilder.Entity<Seat>().ToTable("Seat");

            modelBuilder.Entity<Seat>()
                .Property(s => s.Class)
                .HasConversion<string>();

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Aircraft)
                .WithMany()
                .HasForeignKey(s => s.AircraftId);
        }
    }


}
