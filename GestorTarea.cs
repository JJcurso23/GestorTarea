using System;

namespace Tareas
{
    public class GestorTarea
    {
        public Dictionary<int, Tarea> Tareas { get; set; }
        public GestorTarea()
        {
            Tareas = new Dictionary<int, Tarea>();
        }
        public void AgregarTarea(Tarea tarea)
        {
            if (!Tareas.ContainsKey(tarea.ID)) {
                Tareas.Add(tarea.ID, tarea);
            }
        }
        public void EliminarTarea(int id)
        {
            Tareas.Remove(id);
        }
        public List<Tarea> ObtenerTareas()
        {
            return Tareas.Values.ToList();
        }

    }
}


