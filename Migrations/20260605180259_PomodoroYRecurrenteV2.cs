using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTarea.Migrations
{
    /// <inheritdoc />
    public partial class PomodoroYRecurrenteV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalosEnDias",
                table: "TareasRecurrentes");

            migrationBuilder.AddColumn<int>(
                name: "IntervaloDias",
                table: "TareasRecurrentes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TiempoInit",
                table: "TareasPomodoro",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TiempoFinal",
                table: "TareasPomodoro",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DuracionMinutos",
                table: "TareasPomodoro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sesiones",
                table: "TareasPomodoro",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervaloDias",
                table: "TareasRecurrentes");

            migrationBuilder.DropColumn(
                name: "DuracionMinutos",
                table: "TareasPomodoro");

            migrationBuilder.DropColumn(
                name: "Sesiones",
                table: "TareasPomodoro");

            migrationBuilder.AddColumn<string>(
                name: "IntervalosEnDias",
                table: "TareasRecurrentes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TiempoInit",
                table: "TareasPomodoro",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TiempoFinal",
                table: "TareasPomodoro",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
