using System;

namespace Tareas
{
    public class TareaUrgente: Tarea
    {

        public string Responsable;
        public TareaUrgente(string titulo, string descrip,
            DateTime endDay, string responsable)
            :base(string titulo, string descrip, DateTime endDay )
        {
            this.Responsable = responsable;
        }

        public void NotificarResponsable()
        {
            Console.WriteLine($"Notificar a {Responsable} por SMS");
        }
    }
}

