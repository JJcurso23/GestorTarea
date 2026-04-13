using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tareas
{
    public class GestorTarea
    {
        private readonly Dictionary<int, Tarea> _tareas;
        public GestorTarea()
        {
            _tareas = new Dictionary<int, Tarea>();
        }
        public void AgregarTarea(Tarea tarea)
        {
            if (tarea == null) throw new ArgumentNullException(nameof(tarea));

            if (!_tareas.ContainsKey(tarea.ID)) {
                _tareas.Add(tarea.ID, tarea);
            }
            else
            {
                Console.WriteLine($"Error: Ya existe una tarea con el ID");
            }
        }
        public void EliminarTarea(int id)
        {
            _tareas.Remove(id);
        }
        public IEnumerable<Tarea> ObtenerTareas()
        {
            return _tareas.Values.ToList();
        }

        public Tarea ObtenerPorId(int id)
        {
            _tareas.TryGetValue(id, out var tarea);
            return tarea;
        }

        public IEnumerable<Tarea> Buscar(Func<Tarea, bool> criterio)
        {
            //var resultados = gestor.Buscar(t => t.titulo.Contains("Examen"));
            //var vencenHoy = gestor.Buscar(t => t.Endday.Date == DateTime.Today);
            //var pendientes = gestor.Buscar(t => t.Estado == EstadoTarea.Pendiente);
            return _tareas.Values.Where(criterio);
        }

    }
}


