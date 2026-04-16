using System;

namespace Tareas {
    public class TareaDTo
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Description { get; set; }
        public DateTime FechaLimite { get; set; }
        public int Prioridad { get; set; }
        public required string Estado { get; set; }
        public int UsuarioID { get; set; }
    }
}

