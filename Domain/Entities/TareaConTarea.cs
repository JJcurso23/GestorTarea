using System;

namespace GestorTarea.Domain.Entities
{
    
    public class TareaConTarea: Tarea
    {
        public Dictionary<int, Tarea> Subtareas { get; set; }

        public TareaConTarea(string titulo, string description, DateTime endDay, int usuarioID)
                : base(titulo, description, endDay, usuarioID)
        {
            Subtareas = new Dictionary<int, Tarea>();
        }

        public void AgregarSubTarea(Tarea tarea)
        {
            Subtareas.Add(tarea.ID, tarea);
        }
        public void EliminarSubtarea(int id)
        {
            Subtareas.Remove(id);
        }

        public void MarcarTodasCompletas()
        {
            
            foreach (var t in Subtareas.Values)
            {
                t.CompletarTarea();
            }
        }

        public void ObtenerTareasPendientes()
        {
            var pendientes = Subtareas.Values.Where(t => t.Estado == Enums.EstadoTarea.Pendiente);
            foreach (var t in pendientes) t.ObtenerResumen();
        }

        public void ObtenerTareasCompletas()
        {
            var completas = Subtareas.Values.Where(t => t.Estado == Enums.EstadoTarea.Completada);
            foreach (var t in completas) t.ObtenerResumen();
        }

        public override void ObtenerResumen()
        {
            base.ObtenerResumen();
            Console.WriteLine($"      -> Esta tarea tiene {Subtareas.Count} subtareas.");
        }
    }
}

