using System;

namespace Tareas
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; }
    }
}

