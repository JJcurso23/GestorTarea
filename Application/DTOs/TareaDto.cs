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
    }
}