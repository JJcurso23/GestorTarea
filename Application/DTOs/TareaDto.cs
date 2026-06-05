using System;

namespace GestorTarea.Application.DTOs
{
    // 1. DTO DE ENTRADA (Para POST y PUT - Lo que el cliente envía)
    public class TareaDTO
    {
        // NO tiene Id porque lo asigna SQL
        public required string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public required string Estado { get; set; }
        public int UsuarioID { get; set; }
        public required string TipoTarea { get; set; }

        // --- Campos especificos por tipo (opcionales) ---

        /// <summary>Solo para Pomodoro: duracion de cada sesion en minutos.</summary>
        public int? DuracionMinutos { get; set; }

        /// <summary>Solo para Recurrente: cada cuantos dias se regenera.</summary>
        public int? IntervaloDias { get; set; }
    }

    // 2. DTO DE SALIDA (Para GET - Lo que la API devuelve)
    public class TareaResponseDTO
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public required string Estado { get; set; }
        public int UsuarioID { get; set; }
        public required string TipoTarea { get; set; }

        /// <summary>Nombre del usuario propietario. Se rellena al listar para admin.</summary>
        public string? UsuarioNombre { get; set; }

        // --- Campos especificos por tipo (opcionales) ---

        /// <summary>Solo para Pomodoro.</summary>
        public int? DuracionMinutos { get; set; }

        /// <summary>Solo para Pomodoro: numero de sesiones completadas.</summary>
        public int? Sesiones { get; set; }

        /// <summary>Solo para Pomodoro: timestamp UTC de inicio de la sesion activa, si hay una.</summary>
        public DateTime? TiempoInit { get; set; }

        /// <summary>Solo para Pomodoro: timestamp UTC en el que la sesion activa debe terminar.</summary>
        public DateTime? TiempoFinal { get; set; }

        /// <summary>Solo para Recurrente: cada cuantos dias se regenera.</summary>
        public int? IntervaloDias { get; set; }
    }
}
