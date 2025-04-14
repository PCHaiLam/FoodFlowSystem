using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFlowSystem.Migrations
{
    /// <inheritdoc />
    public partial class remove_p_order_13042025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFoodOrder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HasReservation",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFoodOrder",
                table: "Orders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasReservation",
                table: "Orders",
                type: "bit",
                nullable: true);
        }
    }
}
