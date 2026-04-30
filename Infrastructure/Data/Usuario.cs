using GestorTarea.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Infrastructure.Data
{
    public class Usuario
    {
        public int Id { get; set; } // →PRIMARY KEY automática
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EsAdmin { get; set; }

        // Propiedad de navegación: no genera columna en la BD
        public List<Tarea> Tareas { get; set; } = new();
    }
}
