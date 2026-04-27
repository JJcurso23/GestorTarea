using System;

namespace GestorTarea.Application.DTOs {
    public class TareaDTO
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int Prioridad { get; set; }
        public required string Estado { get; set; }
        public int UsuarioID { get; set; }
    }
}

