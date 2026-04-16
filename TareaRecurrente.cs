using System;

namespace Tareas
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

        }
    }
}

