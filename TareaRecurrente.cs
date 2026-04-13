using System;

namespace Tareas
{
    public class TareaRecurrente : Tarea
    {
        public List<string> IntervalosenDias;
        public TareaRecurrente(string titulo, string descrip,
            DateTime endDay, List<string> intervalosDias)
            : base(string titulo, string descrip, DateTime endDay)
        {
            this.IntervalosenDias = intervalosDias;
        }
        public void SiguienteTarea()
        {

        }
    }
}

