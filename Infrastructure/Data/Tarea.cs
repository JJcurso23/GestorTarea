using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Infrastructure.Data
{
    public abstract class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }   // NULL en BD
        public DateTime? FechaLimite { get; set; }   // NULL en BD
        public bool EstaCompletada { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int UsuarioId { get; set; }          // FK hacia Usuarios
        public Usuario Usuario { get; set; } = null!;
    }
    public class TareaSimple : Tarea { }
    public class TareaRecurrente : Tarea
    {
        public int Intervalo { get; set; }

    }
}