using System;

namespace GestorTarea.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; }

        protected Usuario() { }

        public Usuario(string nombre, string email, int edad, bool activo) {
            this.Nombre = nombre;
            this.Email = email;
            this.Edad = edad;
            this.Activo = activo;
        }
    }
}

