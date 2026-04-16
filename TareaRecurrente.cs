using System;

namespace Tareas
{
    public class TareaRecurrente : Tarea
    {
        public List<string> IntervalosenDias;
        public TareaRecurrente(string titulo, string descrip,
            DateTime endDay, List<string> intervalosDias)
            : base( titulo, descrip, endDay)
        {
            this.IntervalosenDias = intervalosDias;
        }
        public void SiguienteTarea()
        {

        }
    }
}

