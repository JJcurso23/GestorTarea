using System;
using System.Diagnostics.CodeAnalysis;

namespace GestorTarea.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public int Edad { get; set; }
        public bool Activo { get; set; }
        public string PasswordHash { get; set; }
        protected Usuario() { }

        [SetsRequiredMembers]
        public Usuario(string nombre, string email, int edad,string passwordhash, bool activo) {
            this.Nombre = nombre;
            this.Email = email;
            this.Edad = edad;
            this.PasswordHash = passwordhash;
            this.Activo = activo;
        }
    }
}

