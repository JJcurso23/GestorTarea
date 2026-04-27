using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaUrgente: Tarea
    {

        public string Responsable;
        public TareaUrgente(string titulo, string descrip,
            DateTime endDay, int usuarioId, string responsable)
            :base( titulo,  descrip, endDay, usuarioId)
        {
            this.Responsable = responsable;
        }

        public void NotificarResponsable()
        {
            Console.WriteLine($"Notificar a {Responsable} por SMS");
        }
    }
}

