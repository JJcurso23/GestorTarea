using System;

namespace GestorTarea.Application.DTOs {
    public class TareaDTO
    {
        public required string Titulo { get; set; }
        public string? Descripcion { get; set; } // El '?' permite que sea nulo si no viene en el JSON
        public DateTime FechaLimite { get; set; }
        public required string Estado { get; set; }
        public int UsuarioID { get; set; }
        public string TipoTarea { get; set; }
    }
}

