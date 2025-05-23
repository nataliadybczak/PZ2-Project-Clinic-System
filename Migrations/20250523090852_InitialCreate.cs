using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCode.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lekarze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    Specjalizacja = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Haslo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekarze", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pacjenci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    Pesel = table.Column<string>(type: "TEXT", nullable: false),
                    DodanyPrzezId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacjenci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacjenci_Lekarze_DodanyPrzezId",
                        column: x => x.DodanyPrzezId,
                        principalTable: "Lekarze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Choroby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PacjentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choroby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choroby_Pacjenci_PacjentId",
                        column: x => x.PacjentId,
                        principalTable: "Pacjenci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wizyty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PacjentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LekarzId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataWizyty = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wizyty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wizyty_Lekarze_LekarzId",
                        column: x => x.LekarzId,
                        principalTable: "Lekarze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wizyty_Pacjenci_PacjentId",
                        column: x => x.PacjentId,
                        principalTable: "Pacjenci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choroby_PacjentId",
                table: "Choroby",
                column: "PacjentId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacjenci_DodanyPrzezId",
                table: "Pacjenci",
                column: "DodanyPrzezId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacjenci_Pesel",
                table: "Pacjenci",
                column: "Pesel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_LekarzId",
                table: "Wizyty",
                column: "LekarzId");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyty_PacjentId",
                table: "Wizyty",
                column: "PacjentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choroby");

            migrationBuilder.DropTable(
                name: "Wizyty");

            migrationBuilder.DropTable(
                name: "Pacjenci");

            migrationBuilder.DropTable(
                name: "Lekarze");
        }
    }
}
