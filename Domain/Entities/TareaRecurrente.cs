using System;

namespace GestorTarea.Domain.Entities
{
    public class TareaRecurrente : Tarea
    {
        public List<string> IntervalosEnDias { get; set; }

        public TareaRecurrente(string titulo, string descrip, int usuarioID,
            DateTime endDay, List<string> intervalosDias)
            : base(titulo, descrip, endDay, usuarioID)
        {
            this.IntervalosEnDias = intervalosDias;
        }
        public void SiguienteTarea()
        {
            //Todo
        }
        public override void ObtenerResumen()
        {
            Console.WriteLine($"ID: {ID} - Título: {Titulo} - Estado: {Estado} - Cada: {IntervalosEnDias}");
        }
    }
}

