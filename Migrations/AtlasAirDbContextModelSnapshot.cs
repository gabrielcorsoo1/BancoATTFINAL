
using System;
using AtlasAir.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AtlasAir.Migrations
{
    [DbContext(typeof(AtlasAirDbContext))]
    partial class AtlasAirDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AtlasAir.Models.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Aircraft", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Airport", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsSpecialCustomer")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ActualArrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ActualDeparture")
                        .HasColumnType("datetime2");

                    b.Property<int?>("AircraftId")
                        .HasColumnType("int");

                    b.Property<int>("DestinationAirportId")
                        .HasColumnType("int");

                    b.Property<int>("OriginAirportId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduledArrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ScheduledDeparture")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Scheduled");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("DestinationAirportId");

                    b.HasIndex("OriginAirportId");

                    b.ToTable("Flight", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.FlightSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AircraftId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DestinationAirportId")
                        .HasColumnType("int");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int?>("FlightId1")
                        .HasColumnType("int");

                    b.Property<int>("OriginAirportId")
                        .HasColumnType("int");

                    b.Property<byte>("SegmentOrder")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("DestinationAirportId");

                    b.HasIndex("FlightId");

                    b.HasIndex("FlightId1");

                    b.HasIndex("OriginAirportId");

                    b.ToTable("FlightSegment", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CancellationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId1")
                        .HasColumnType("int");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int?>("FlightId1")
                        .HasColumnType("int");

                    b.Property<string>("ReservationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<int?>("SeatId1")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("WaitingPayment");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("CustomerId1");

                    b.HasIndex("FlightId");

                    b.HasIndex("FlightId1");

                    b.HasIndex("SeatId");

                    b.HasIndex("SeatId1");

                    b.ToTable("Reservation", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AircraftId")
                        .HasColumnType("int");

                    b.Property<int?>("AircraftId1")
                        .HasColumnType("int");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("AircraftId1");

                    b.ToTable("Seat", (string)null);
                });

            modelBuilder.Entity("AtlasAir.Models.Flight", b =>
                {
                    b.HasOne("AtlasAir.Models.Aircraft", null)
                        .WithMany("Flights")
                        .HasForeignKey("AircraftId");

                    b.HasOne("AtlasAir.Models.Airport", "DestinationAirport")
                        .WithMany()
                        .HasForeignKey("DestinationAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Airport", "OriginAirport")
                        .WithMany()
                        .HasForeignKey("OriginAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationAirport");

                    b.Navigation("OriginAirport");
                });

            modelBuilder.Entity("AtlasAir.Models.FlightSegment", b =>
                {
                    b.HasOne("AtlasAir.Models.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Airport", "DestinationAirport")
                        .WithMany()
                        .HasForeignKey("DestinationAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Flight", null)
                        .WithMany("FlightSegments")
                        .HasForeignKey("FlightId1");

                    b.HasOne("AtlasAir.Models.Airport", "OriginAirport")
                        .WithMany()
                        .HasForeignKey("OriginAirportId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("DestinationAirport");

                    b.Navigation("Flight");

                    b.Navigation("OriginAirport");
                });

            modelBuilder.Entity("AtlasAir.Models.Reservation", b =>
                {
                    b.HasOne("AtlasAir.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Customer", null)
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId1");

                    b.HasOne("AtlasAir.Models.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Flight", null)
                        .WithMany("Reservations")
                        .HasForeignKey("FlightId1");

                    b.HasOne("AtlasAir.Models.Seat", "Seat")
                        .WithMany()
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Seat", null)
                        .WithMany("Reservations")
                        .HasForeignKey("SeatId1");

                    b.Navigation("Customer");

                    b.Navigation("Flight");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("AtlasAir.Models.Seat", b =>
                {
                    b.HasOne("AtlasAir.Models.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtlasAir.Models.Aircraft", null)
                        .WithMany("Seats")
                        .HasForeignKey("AircraftId1");

                    b.Navigation("Aircraft");
                });

            modelBuilder.Entity("AtlasAir.Models.Aircraft", b =>
                {
                    b.Navigation("Flights");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("AtlasAir.Models.Customer", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("AtlasAir.Models.Flight", b =>
                {
                    b.Navigation("FlightSegments");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("AtlasAir.Models.Seat", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
