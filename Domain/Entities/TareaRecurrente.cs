using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaRecurrente : Tarea
    {
        /// <summary>Cada cuántos días debe regenerarse la tarea cuando se complete.</summary>
        public int IntervaloDias { get; set; } = 1;

        protected TareaRecurrente() : base() { }

        public TareaRecurrente(string titulo, string descrip, DateTime endDay,
            int usuarioID, int intervaloDias)
            : base(titulo, descrip, endDay, usuarioID)
        {
            if (intervaloDias < 1) intervaloDias = 1;
            IntervaloDias = intervaloDias;
        }

        /// <summary>
        /// Crea la siguiente instancia de la tarea recurrente con la fecha
        /// limite empujada IntervaloDias dias en el futuro desde hoy.
        /// </summary>
        public TareaRecurrente GenerarSiguiente()
        {
            return new TareaRecurrente(
                Titulo,
                Descripcion,
                DateTime.Now.AddDays(IntervaloDias),
                UsuarioID,
                IntervaloDias);
        }

        public override void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {Titulo} - Estado: {Estado} - Cada {IntervaloDias} días");
        }
    }
}
