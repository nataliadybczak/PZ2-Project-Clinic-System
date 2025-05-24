using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCode.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenToLekarz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Lekarze",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Lekarze");
        }
    }
}
