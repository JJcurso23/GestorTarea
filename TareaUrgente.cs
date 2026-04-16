using System;

namespace Tareas
{
    public class TareaUrgente: Tarea
    {

        public string Responsable;
        public TareaUrgente(string titulo, string descrip,
            DateTime endDay, string responsable)
            :base( titulo,  descrip, endDay )
        {
            this.Responsable = responsable;
        }

        public void NotificarResponsable()
        {
            Console.WriteLine($"Notificar a {Responsable} por SMS");
        }
    }
}

