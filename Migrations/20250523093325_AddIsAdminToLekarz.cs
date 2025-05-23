using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCode.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAdminToLekarz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Lekarze",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Lekarze");
        }
    }
}
