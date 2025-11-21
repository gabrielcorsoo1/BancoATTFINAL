using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtlasAir.Migrations
{
    /// <inheritdoc />
    public partial class AircraftFlightCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AircraftId",
                table: "Flight",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AircraftId",
                table: "Flight",
                column: "AircraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Aircraft_AircraftId",
                table: "Flight",
                column: "AircraftId",
                principalTable: "Aircraft",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Aircraft_AircraftId",
                table: "Flight");

            migrationBuilder.DropIndex(
                name: "IX_Flight_AircraftId",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "AircraftId",
                table: "Flight");
        }
    }
}
