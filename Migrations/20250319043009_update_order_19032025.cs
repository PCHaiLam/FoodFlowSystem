using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFlowSystem.Migrations
{
    /// <inheritdoc />
    public partial class update_order_19032025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDate",
                table: "Orders",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ReservationTime",
                table: "Orders",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReservationTime",
                table: "Orders");
        }
    }
}
