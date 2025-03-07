using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodFlowSystem.Migrations
{
    /// <inheritdoc />
    public partial class remove_p_status_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
