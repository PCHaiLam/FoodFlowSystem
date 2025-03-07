using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFlowSystem.Migrations
{
    /// <inheritdoc />
    public partial class edit_p_payment_invoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeposit",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReservationID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "Invoices",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Invoices",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReservationID",
                table: "Payments",
                column: "ReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Reservations_ReservationID",
                table: "Payments",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Reservations_ReservationID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReservationID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeposit",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReservationID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoices");
        }
    }
}
