using System;

namespace GestorTarea.Domain.Entities
{
    
    public class TareaConTarea: Tarea
    {
        public Dictionary<int, Tarea> subtarea { get; set; }

        public TareaConTarea(string titulo, string description, DateTime endDay, int usuarioID)
                : base(titulo, description, endDay, usuarioID)
        {
            subtarea = new Dictionary<int, Tarea>();
        }

        public void AgregarSubTarea(Tarea tarea)
        {
            subtarea.Add(tarea.ID, tarea);
        }
        public void EliminarSubtarea(int id)
        {
            subtarea.Remove(id);
        }

        public void MarcarTodasCompletas()
        {
            
            foreach (var t in subtarea.Values)
            {
                t.CompletarTarea();
            }
        }

        public void ObtenerTareasPendientes()
        {
            var pendientes = subtarea.Values.Where(t => t.Estado == EstadoTarea.Pendiente);
            foreach (var t in pendientes) t.ObtenerResumen();
        }

        public void ObtenerTareasCompletas()
        {
            var completas = subtarea.Values.Where(t => t.Estado == EstadoTarea.Completada);
            foreach (var t in completas) t.ObtenerResumen();
        }

        public override void ObtenerResumen()
        {
            base.ObtenerResumen();
            Console.WriteLine($"      -> Esta tarea tiene {subtarea.Count} subtareas.");
        }
    }
}

