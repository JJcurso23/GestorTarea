using System;
using System.Text.Json;
using System.IO;

namespace Tareas
{
    public class GestorTarea
    {
        public Dictionary<int, Tarea> Tareas { get; set; }
        public GestorTarea()
        {
            Tareas = new Dictionary<int, Tarea>();
        }
        public bool AgregarTarea(Tarea tarea)
        {
            if (!Tareas.ContainsKey(tarea.ID)) {
                Tareas.Add(tarea.ID, tarea);
                return true;
            }
            return false;
        }
        public void EliminarTarea(int id)
        {
            Tareas.Remove(id);
        }
        public List<Tarea> ObtenerTareas()
        {
            return Tareas.Values.ToList();
        }

        public void Guardar(string ruta)
        {
            var ListaDto = new List<TareaDTo>();

            foreach (var tarea in Tareas.Values)
            {
                ListaDto.Add(new TareaDTo
                {
                    Id = tarea.ID,
                    Titulo = tarea.titulo,
                    FechaLimite = tarea.Endday,
                    Prioridad = 0,
                    Estado = tarea.Estado.ToString()
                });
            }

            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(ListaDto, opciones);
            File.WriteAllText(ruta, json);

        }

        public void Cargar(string ruta)
        {
            if (!File.Exists(ruta))
            {
                Tareas = new Dictionary<int, Tarea>();
                return;
            }
            string json = File.ReadAllText(ruta);
            var listaDto = JsonSerializer.Deserialize<List<TareaDTo>>(json);
            Tareas = new Dictionary<int, Tarea>();
            foreach(var dto in listaDto)
            {
                TareaSimple tarea = new TareaSimple(
                    dto.Titulo,
                    "",
                    dto.FechaLimite);

                Tareas.Add(tarea.ID, tarea);
            }

        }
    }
}


