using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaRecurrente : Tarea
    {
        public List<string> IntervalosenDias;
        public TareaRecurrente(string titulo, string descrip, int usuarioID,
            DateTime endDay, List<string> intervalosDias)
            : base(titulo, descrip, endDay, usuarioID)
        {
            this.IntervalosenDias = intervalosDias;
        }
        public void SiguienteTarea()
        {
            //Todo
        }
        public virtual void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {Titulo} - Estado: {Estado} - Cada: {IntervalosenDias}");
        }
    }
}

