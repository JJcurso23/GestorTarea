using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTarea.Migrations
{
    /// <inheritdoc />
    public partial class InicialTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diainicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.TareaID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "TareasConTarea",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    TareaDependienteID = table.Column<int>(type: "int", nullable: false),
                    InstruccionesDependencia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasConTarea", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_TareasConTarea_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareasPomodoro",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    TiempoFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TiempoInit = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasPomodoro", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_TareasPomodoro_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareasRecurrentes",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    IntervalosEnDias = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasRecurrentes", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_TareasRecurrentes_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareasSimples",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasSimples", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_TareasSimples_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareasUrgentes",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasUrgentes", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_TareasUrgentes_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TareasConTarea");

            migrationBuilder.DropTable(
                name: "TareasPomodoro");

            migrationBuilder.DropTable(
                name: "TareasRecurrentes");

            migrationBuilder.DropTable(
                name: "TareasSimples");

            migrationBuilder.DropTable(
                name: "TareasUrgentes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Tareas");
        }
    }
}
