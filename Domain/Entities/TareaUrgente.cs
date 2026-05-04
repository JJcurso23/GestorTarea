using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaUrgente: Tarea
    {

        protected TareaUrgente() : base() { }
        public TareaUrgente(string titulo, string descrip,
            DateTime diaVencimiento, int usuarioId, string responsable)
            :base( titulo,  descrip, diaVencimiento, usuarioId)
        {}

        public void NotificarResponsable()
        {
            Console.WriteLine($"Notificar a {Responsable} por SMS");
        }

        public override void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {Titulo} - Estado: {Estado} - Vence: {DiaVencimiento.ToString()}");
        }
    }
}

